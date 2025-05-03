using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework.Logic.UI;
using Proto.Guild;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class UIGuildShopItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		public RectTransform rt
		{
			get
			{
				return base.rectTransform;
			}
		}

		protected override void GuildUI_OnInit()
		{
			this.aniRt = this.canvasGroup.GetComponent<RectTransform>();
			if (this.animator != null)
			{
				this.animator.enabled = false;
			}
			this.goodsItemObj.SetActiveSafe(false);
			DelayCall.Instance.ClearCall(new DelayCall.CallAction(this.RefreshTime));
			DelayCall.Instance.CallLoop(1000, new DelayCall.CallAction(this.RefreshTime));
			this.buttonRefresh.onClick.AddListener(new UnityAction(this.OnClickRefresh));
		}

		protected override void GuildUI_OnUnInit()
		{
			if (this.buttonRefresh != null)
			{
				this.buttonRefresh.onClick.RemoveListener(new UnityAction(this.OnClickRefresh));
			}
			for (int i = 0; i < this.goodsItemList.Count; i++)
			{
				this.goodsItemList[i].DeInit();
				Object.Destroy(this.goodsItemList[i].gameObject);
			}
			this.goodsItemList.Clear();
			this.ClearTime();
		}

		public void Refresh(GuildShopType type)
		{
			this.shopType = type;
			this.group = base.SDK.GuildShop.GetShopGroup((int)this.shopType);
			string text = GuildProxy.Language.GetInfoByID("400203");
			if (this.shopType == GuildShopType.Daily)
			{
				text = GuildProxy.Language.GetInfoByID("400183");
			}
			else if (this.shopType == GuildShopType.Weekly)
			{
				text = GuildProxy.Language.GetInfoByID("400184");
			}
			this.textName.text = text;
			this.buttonRefresh.gameObject.SetActive(this.group != null && this.group.RefreshShopCost != null);
			this.RefreshGoods();
			this.RefreshTime();
		}

		private void RefreshGoods()
		{
			List<GuildShopData> list = new List<GuildShopData>();
			if (this.group != null)
			{
				list = this.group.ShopList;
			}
			for (int i = 0; i < this.goodsItemList.Count; i++)
			{
				this.goodsItemList[i].gameObject.SetActiveSafe(false);
			}
			for (int j = 0; j < list.Count; j++)
			{
				UIGuildShopGoodsItem uiguildShopGoodsItem;
				if (j < this.goodsItemList.Count)
				{
					uiguildShopGoodsItem = this.goodsItemList[j];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.goodsItemObj);
					gameObject.transform.SetParentNormal(this.gridLayout.gameObject, false);
					uiguildShopGoodsItem = gameObject.GetComponent<UIGuildShopGoodsItem>();
					uiguildShopGoodsItem.Init();
					this.goodsItemList.Add(uiguildShopGoodsItem);
				}
				uiguildShopGoodsItem.gameObject.SetActiveSafe(true);
				uiguildShopGoodsItem.Refresh(list[j]);
			}
			float num = (float)(this.gridLayout.padding.top + this.gridLayout.padding.bottom);
			int num2 = list.Count / this.gridLayout.constraintCount + ((list.Count % this.gridLayout.constraintCount > 0) ? 1 : 0);
			float num3 = num + (float)(num2 * 400) + (float)(num2 - 1) * this.gridLayout.spacing.y + 250f;
			this.rt.sizeDelta = new Vector2(this.rt.sizeDelta.x, num3);
		}

		private void RefreshTime()
		{
			GuildShopGroup shopGroup = base.SDK.GuildShop.GetShopGroup((int)this.shopType);
			long num = (long)(((shopGroup != null) ? shopGroup.ShopRefreshTime : 0UL) - (ulong)GuildProxy.Net.ServerTime());
			num = ((num > 0L) ? num : 0L);
			string time = Singleton<LanguageManager>.Instance.GetTime(num);
			this.textTime.text = GuildProxy.Language.GetInfoByID1("400185", time);
		}

		private void OnSendRefreshMsg()
		{
			GuildNetUtil.Guild.DoRequest_GuildShopRefresh(this.shopType, delegate(bool result, GuildShopRefreshResponse resp)
			{
				if (result)
				{
					this.RefreshGoods();
				}
			});
		}

		private bool IsRefreshFree()
		{
			return this.group != null && this.group.RefreshShopCost != null && this.group.RefreshShopCost.count == 0;
		}

		public void ClearAni()
		{
			this.canvasGroup.alpha = 1f;
			this.aniRt.anchoredPosition = Vector2.zero;
			if (this.animator != null)
			{
				this.animator.enabled = false;
			}
		}

		public void ShowAni(int index)
		{
			if (this.animator == null)
			{
				return;
			}
			this.animator.enabled = false;
			this.canvasGroup.alpha = 0f;
			DelayCall.Instance.CallOnce(index * 50, new DelayCall.CallAction(this.OnShowAni));
		}

		private void OnShowAni()
		{
			if (this.animator == null)
			{
				return;
			}
			this.animator.enabled = true;
			this.animator.Play("UIGuildHallMemberItem_Show");
		}

		public void ClearTime()
		{
			DelayCall.Instance.ClearCall(new DelayCall.CallAction(this.RefreshTime));
		}

		private void OnClickRefresh()
		{
			if (this.group != null)
			{
				if (this.IsRefreshFree())
				{
					this.OnSendRefreshMsg();
					return;
				}
				GuildProxy.UI.ShowBuyPop(GuildProxy.Language.GetInfoByID("400209"), GuildProxy.Language.GetInfoByID("400210"), this.group.RefreshShopCost, new Action(this.OnSendRefreshMsg));
			}
		}

		public CustomText textName;

		public CustomText textTime;

		public GridLayoutGroup gridLayout;

		public GameObject goodsItemObj;

		public CustomButton buttonRefresh;

		public CanvasGroup canvasGroup;

		public Animator animator;

		private List<UIGuildShopGoodsItem> goodsItemList = new List<UIGuildShopGoodsItem>();

		private GuildShopType shopType;

		private GuildShopGroup group;

		private RectTransform aniRt;
	}
}
