using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Pay;
using UnityEngine;

namespace HotFix.VIPUI
{
	public class VIPRewardsUI : CustomBehaviour
	{
		private int CurShowVIPLevel
		{
			get
			{
				if (this.mCurShowVIPData != null)
				{
					return this.mCurShowVIPData.m_id;
				}
				return 0;
			}
		}

		protected override void OnInit()
		{
			this.isInit = true;
			this.mVIPDataModule = GameApp.Data.GetDataModule(DataName.VIPDataModule);
			this.ButtonFree.m_onClick = new Action(this.OnGetRewardsFree);
			this.ButtonBuy.m_onClick = new Action(this.OnBuyRewards);
		}

		protected override void OnDeInit()
		{
			this.isInit = false;
			this.ButtonFree.m_onClick = null;
			this.ButtonBuy.m_onClick = null;
		}

		public void SetData(VIPDataModule.VIPDatas data)
		{
			this.mCurShowVIPData = data;
		}

		public void RefreshUI()
		{
			this.mRewardDataList.Clear();
			int num = this.mBuyCostItemID;
			this.mBuyCostItemID = 0;
			this.mBuyCostItemCount = 0;
			this.mBuyCostItemCountOld = 0;
			if (this.mCurShowVIPData != null)
			{
				this.mBuyCostItemID = (int)this.mCurShowVIPData.BuyRewardsCost.id;
				this.mBuyCostItemCount = (int)this.mCurShowVIPData.BuyRewardsCost.count;
				this.mBuyCostItemCountOld = (int)this.mCurShowVIPData.BuyRewardsCostOld.count;
				this.mRewardDataList.AddRange(this.mCurShowVIPData.BuyRewards);
			}
			int num2 = ((this.mBuyCostItemID != num) ? this.mBuyCostItemID : 0);
			this.RefreshCost(num2);
			this.RefreshRewardsList();
			this.TitleLayoutUI.RefreshUI();
		}

		private void RefreshRewardsList()
		{
			for (int i = 0; i < this.mRewardDataList.Count; i++)
			{
				PropData propData = this.mRewardDataList[i];
				UIItem uiitem = null;
				if (i < this.mItemUIList.Count)
				{
					uiitem = this.mItemUIList[i];
				}
				else if (uiitem == null)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.PrefabItem, this.RTFRewards);
					gameObject.SetActive(true);
					uiitem = gameObject.GetComponent<UIItem>();
					uiitem.onClick = new Action<UIItem, PropData, object>(this.OnClickItem);
					uiitem.Init();
					this.mItemUIList.Add(uiitem);
				}
				uiitem.SetData(propData);
				uiitem.OnRefresh();
				uiitem.SetActive(true);
			}
			for (int j = this.mRewardDataList.Count; j < this.mItemUIList.Count; j++)
			{
				this.mItemUIList[j].SetActive(false);
			}
		}

		private void RefreshCost(int costitemid = 0)
		{
			if (costitemid != 0)
			{
				Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(this.mBuyCostItemID);
				this.ImagePrice.SetImage(GameApp.Table.GetAtlasPath(elementById.atlasID), elementById.icon);
				this.ImagePriceOld.SetImage(GameApp.Table.GetAtlasPath(elementById.atlasID), elementById.icon);
			}
			this.TextPrice.text = this.mBuyCostItemCount.ToString();
			Vector2 vector = this.TextPrice.rectTransform.sizeDelta;
			vector.x = this.TextPrice.preferredWidth;
			this.TextPrice.rectTransform.sizeDelta = vector;
			this.TextPriceOld.text = this.mBuyCostItemCountOld.ToString();
			vector = this.TextPriceOld.rectTransform.sizeDelta;
			vector.x = this.TextPriceOld.preferredWidth;
			this.TextPriceOld.rectTransform.sizeDelta = vector;
			bool flag = this.mVIPDataModule.IsRewardGetted(this.CurShowVIPLevel);
			this.ButtonFree.gameObject.SetActive(this.mBuyCostItemCount <= 0 && !flag);
			this.ButtonBuy.gameObject.SetActive(this.mBuyCostItemCount > 0 && !flag);
			this.ObjRewardsGetted.SetActive(flag);
			bool flag2 = this.mVIPDataModule.IsCanGetReward(this.mCurShowVIPData.m_id);
			this.RedNodeFree.Value = (flag2 ? 1 : 0);
			this.RedNodeBuy.Value = (flag2 ? 1 : 0);
		}

		private void OnClickItem(UIItem item, PropData data, object arg3)
		{
			DxxTools.UI.OnItemClick(item, data, arg3);
		}

		private void OnGetRewardsFree()
		{
			if (this.mCurShowVIPData == null)
			{
				return;
			}
			if (this.mVIPDataModule.VipLevel < this.mCurShowVIPData.m_id)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("8811"));
				return;
			}
			this.InternalGetRewards();
		}

		private void OnBuyRewards()
		{
			if (this.mCurShowVIPData == null)
			{
				return;
			}
			if (this.mVIPDataModule.VipLevel < this.mCurShowVIPData.m_id)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("8811"));
				return;
			}
			if (this.mCurShowVIPData.BuyRewardsCost == null)
			{
				this.OnGetRewardsFree();
				return;
			}
			int id = (int)this.mCurShowVIPData.BuyRewardsCost.id;
			int num = (int)this.mCurShowVIPData.BuyRewardsCost.count;
			if (!DxxTools.UI.CheckCurrencyAndShowTips(id, num, true))
			{
				return;
			}
			this.InternalGetRewards();
		}

		private void InternalGetRewards()
		{
			if (this.mCurShowVIPData == null)
			{
				return;
			}
			NetworkUtils.Purchase.SendVIPLevelRewardRequest(this.mCurShowVIPData.m_id, delegate(bool result, VIPLevelRewardResponse resp)
			{
				if (!this.isInit)
				{
					return;
				}
				if (result)
				{
					if (resp.CommonData.Reward != null)
					{
						DxxTools.UI.OpenRewardCommon(resp.CommonData.Reward, null, true);
					}
					if (base.gameObject != null && base.gameObject.activeInHierarchy)
					{
						this.RefreshUI();
					}
				}
			});
		}

		public RectTransform RTFRewards;

		public GameObject PrefabItem;

		public CustomButton ButtonFree;

		public RedNodeOneCtrl RedNodeFree;

		public CustomButton ButtonBuy;

		public RedNodeOneCtrl RedNodeBuy;

		public GameObject ObjRewardsGetted;

		public CustomImage ImagePriceOld;

		public CustomImage ImagePrice;

		public CustomText TextPriceOld;

		public CustomText TextPrice;

		public GameObject ObjPriceOld;

		public VIPRewardsTitleLayoutUI TitleLayoutUI;

		private List<PropData> mRewardDataList = new List<PropData>();

		private List<UIItem> mItemUIList = new List<UIItem>();

		private VIPDataModule mVIPDataModule;

		private VIPDataModule.VIPDatas mCurShowVIPData;

		private int mBuyCostItemID;

		private int mBuyCostItemCount;

		private int mBuyCostItemCountOld;

		private bool isInit;
	}
}
