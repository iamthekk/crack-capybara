using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework.EventSystem;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class UIGuildHallShopCtrl : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.shopItemObj.SetActiveSafe(false);
			foreach (int num in base.SDK.GuildShop.GetShopGropDic().Keys)
			{
				UIGuildShopItem uiguildShopItem = this.CreateShopItem();
				this.shopItemDic.Add(num, uiguildShopItem);
			}
		}

		protected override void GuildUI_OnShow()
		{
			GuildProxy.GameEvent.AddReceiver(LocalMessageName.CC_Guild_RefreshShop, new HandlerEvent(this.OnRefresh));
			base.SDK.Event.RegisterEvent(19, new GuildHandlerEvent(this.OnGuildRefresh));
			base.gameObject.SetActiveSafe(true);
			this.Refresh();
		}

		protected override void GuildUI_OnClose()
		{
			base.gameObject.SetActiveSafe(false);
			GuildProxy.GameEvent.RemoveReceiver(LocalMessageName.CC_Guild_RefreshShop, new HandlerEvent(this.OnRefresh));
			base.SDK.Event.UnRegisterEvent(19, new GuildHandlerEvent(this.OnGuildRefresh));
			foreach (UIGuildShopItem uiguildShopItem in this.shopItemDic.Values)
			{
				uiguildShopItem.ClearTime();
			}
			this.isShowAni = false;
		}

		protected override void GuildUI_OnUnInit()
		{
			foreach (UIGuildShopItem uiguildShopItem in this.shopItemDic.Values)
			{
				uiguildShopItem.DeInit();
				Object.Destroy(uiguildShopItem.gameObject);
			}
			this.shopItemDic.Clear();
		}

		private void Refresh()
		{
			List<UIGuildShopItem> list = new List<UIGuildShopItem>();
			int num = 0;
			foreach (int num2 in this.shopItemDic.Keys)
			{
				UIGuildShopItem uiguildShopItem = this.shopItemDic[num2];
				uiguildShopItem.Refresh((GuildShopType)num2);
				uiguildShopItem.ClearAni();
				list.Add(uiguildShopItem);
				GuildShopGroup shopGroup = base.SDK.GuildShop.GetShopGroup(num2);
				if (shopGroup != null)
				{
					num += shopGroup.ShopList.Count;
				}
			}
			if (num == 0)
			{
				this.textTip.text = GuildProxy.Language.GetInfoByID("400188");
			}
			else
			{
				this.textTip.text = "";
			}
			if (this.isShowAni)
			{
				this.isShowAni = false;
				for (int i = 0; i < list.Count; i++)
				{
					list[i].ShowAni(i);
				}
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.layoutRT);
		}

		public void ShowAni()
		{
			this.shopScroll.content.anchoredPosition = Vector2.zero;
			this.isShowAni = true;
		}

		private UIGuildShopItem CreateShopItem()
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.shopItemObj);
			gameObject.transform.SetParentNormal(this.shopScroll.content.gameObject, false);
			UIGuildShopItem component = gameObject.GetComponent<UIGuildShopItem>();
			component.Init();
			component.gameObject.SetActiveSafe(true);
			return component;
		}

		private void OnGuildRefresh(int type, GuildBaseEvent eventArgs)
		{
			this.Refresh();
		}

		private void OnRefresh(object sender, int type, BaseEventArgs eventArgs)
		{
			this.Refresh();
		}

		public ScrollRect shopScroll;

		public GameObject shopItemObj;

		public CustomText textTip;

		public RectTransform layoutRT;

		private Dictionary<int, UIGuildShopItem> shopItemDic = new Dictionary<int, UIGuildShopItem>();

		private bool isShowAni;
	}
}
