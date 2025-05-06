using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using SuperScrollView;
using UnityEngine;

namespace HotFix
{
	public class ItemResourcesItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.ScrollView_Reward.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			this.Button_Go.m_onClick = new Action(this.OnClickButtonGoTo);
			this.Button_Buy.m_onClick = new Action(this.OnClickButtonBuy);
		}

		protected override void OnDeInit()
		{
			this.Button_Go.m_onClick = null;
			this.Button_Buy.m_onClick = null;
		}

		private void OnClickButtonGoTo()
		{
			if (this.m_itemResourcesCfg == null)
			{
				return;
			}
			this.JumpToView((ViewJumpType)this.m_paramsId);
		}

		private async void JumpToView(ViewJumpType jumpType)
		{
			if (Singleton<ViewJumpCtrl>.Instance.IsCanJumpTo(jumpType, null, true))
			{
				await Singleton<ViewJumpCtrl>.Instance.JumpTo(jumpType, null);
				GameApp.View.CloseView(ViewName.ItemResourcesViewModule, null);
			}
		}

		private void OnClickButtonBuy()
		{
			if (this.m_itemResourcesCfg == null)
			{
				return;
			}
			if (this.m_paramsId == 9)
			{
				CommonTicketDailyExchangeTipModule.OpenData openData = default(CommonTicketDailyExchangeTipModule.OpenData);
				openData.SetData(UserTicketKind.UserLife);
				GameApp.View.OpenView(ViewName.CommonTicketDailyExchangeTipModule, openData, 1, null, null);
				return;
			}
			CommonTicketBuyTipModule.OpenData openData2 = default(CommonTicketBuyTipModule.OpenData);
			openData2.SetData((UserTicketKind)this.m_paramsId);
			GameApp.View.OpenView(ViewName.CommonTicketBuyTipModule, openData2, 1, null, null);
		}

		public void SetData(ItemResources_itemget cfg, int jumpId, Action<bool> OnBuySupplyGiftSucess, Action OnBuySupplyGiftClose)
		{
			if (cfg == null)
			{
				return;
			}
			this.m_itemResourcesCfg = cfg;
			this.m_jumpResourcesCfg = GameApp.Table.GetManager().GetItemResources_jumpResource(jumpId);
			if (this.m_jumpResourcesCfg == null)
			{
				return;
			}
			this.m_OnBuySupplyGiftSuccess = OnBuySupplyGiftSucess;
			this.m_OnBuySupplyGiftClose = OnBuySupplyGiftClose;
			this.m_itemResourcesType = this.m_jumpResourcesCfg.jumpType;
			this.m_paramsId = this.m_jumpResourcesCfg.jumpId;
			int num = 0;
			string text = "";
			string text2 = "";
			this.m_giftData = null;
			this.m_supplyData = null;
			if (this.m_itemResourcesType == ItemResourcesType.Gift.GetHashCode())
			{
				this.Image_SpecialBg.gameObject.SetActiveSafe(true);
				this.Image_NormalBg.gameObject.SetActiveSafe(false);
				this.Off.gameObject.SetActiveSafe(false);
				this.Text_GiftTitle.gameObject.SetActiveSafe(true);
				this.Text_Title.gameObject.SetActiveSafe(false);
				this.Button_Price.gameObject.SetActiveSafe(false);
				this.Button_Buy.gameObject.SetActiveSafe(false);
				this.Button_Go.gameObject.SetActiveSafe(false);
				num = this.m_jumpResourcesCfg.atlas;
				text = this.m_jumpResourcesCfg.icon;
				List<IAP_pushIap> list = (from x in GameApp.Table.GetManager().GetIAP_pushIapElements()
					where x.@group == this.m_paramsId
					select x).ToList<IAP_pushIap>();
				PushGiftDataModule dataModule = GameApp.Data.GetDataModule(DataName.PushGiftDataModule);
				if (dataModule != null)
				{
					this.m_giftData = dataModule.OnGetSupplyData(list[0].conditionParams);
					this.m_supplyData = dataModule.OnGetSupplyBaseData(list[0].conditionParams);
					if (this.m_giftData != null)
					{
						this.ScrollView_Reward.SetListItemCount(this.m_giftData.Items.Count, false);
						this.ScrollView_Reward.RefreshAllShowItems();
						this.Button_Price.gameObject.SetActiveSafe(true);
						text2 = this.m_giftData.PurchaseConfig.nameID;
						if (this.m_giftData.PurchaseConfig.valueBet != 0)
						{
							this.Off.gameObject.SetActiveSafe(true);
							this.Text_OffValue.text = string.Format("{0}%", this.m_giftData.PurchaseConfig.valueBet * 100);
							this.PayButtonCtrl.SetData(this.m_giftData.ConfigId, "", new Action<bool>(this.OnPaySuccess), new Action(this.OnCloseReward));
						}
						else
						{
							this.Off.gameObject.SetActiveSafe(false);
						}
					}
				}
			}
			else if (this.m_itemResourcesType == ItemResourcesType.Diamond.GetHashCode())
			{
				this.Image_SpecialBg.gameObject.SetActiveSafe(false);
				this.Image_NormalBg.gameObject.SetActiveSafe(true);
				this.Off.gameObject.SetActiveSafe(false);
				this.Text_GiftTitle.gameObject.SetActiveSafe(false);
				this.Text_Title.gameObject.SetActiveSafe(true);
				this.Button_Price.gameObject.SetActiveSafe(false);
				this.Button_Buy.gameObject.SetActiveSafe(true);
				this.Button_Go.gameObject.SetActiveSafe(false);
				num = this.m_jumpResourcesCfg.atlas;
				text = this.m_jumpResourcesCfg.icon;
				text2 = this.m_jumpResourcesCfg.language;
			}
			else if (this.m_itemResourcesType == ItemResourcesType.GoTo.GetHashCode())
			{
				this.Image_SpecialBg.gameObject.SetActiveSafe(false);
				this.Image_NormalBg.gameObject.SetActiveSafe(true);
				this.Off.gameObject.SetActiveSafe(false);
				this.Text_GiftTitle.gameObject.SetActiveSafe(false);
				this.Text_Title.gameObject.SetActiveSafe(true);
				this.Button_Price.gameObject.SetActiveSafe(false);
				this.Button_Buy.gameObject.SetActiveSafe(false);
				this.Button_Go.gameObject.SetActiveSafe(true);
				num = this.m_jumpResourcesCfg.atlas;
				text = this.m_jumpResourcesCfg.icon;
				text2 = this.m_jumpResourcesCfg.language;
			}
			this.Text_Title.text = Singleton<LanguageManager>.Instance.GetInfoByID(text2);
			this.Text_GiftTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(text2);
			this.Image_Icon.SetImage(GameApp.Table.GetAtlasPath(num), text);
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("RewardItem");
			UIItem component = loopListViewItem.transform.GetChild(0).GetComponent<UIItem>();
			if (!loopListViewItem.IsInitHandlerCalled)
			{
				component.Init();
				loopListViewItem.IsInitHandlerCalled = true;
			}
			ItemData itemData = this.m_giftData.Items[index];
			component.SetData(itemData.ToPropData());
			component.OnRefresh();
			component.SetActive(true);
			return loopListViewItem;
		}

		private void OnPaySuccess(bool isOk)
		{
			if (isOk)
			{
				if (this.m_supplyData != null)
				{
					GameApp.Data.GetDataModule(DataName.PushGiftDataModule)._popingData = this.m_supplyData;
				}
				this._isOk = true;
			}
		}

		private void OnCloseReward()
		{
			if (!this._isOk)
			{
				return;
			}
			if (this.m_giftData.PushConfig.type == 1)
			{
				EventArgsBuySupplyGiftSuccess instance = Singleton<EventArgsBuySupplyGiftSuccess>.Instance;
				instance.ConfigId = this.m_giftData.PushConfig.conditionParams;
				GameApp.Event.DispatchNow(null, 251, instance);
				Action onBuySupplyGiftClose = this.m_OnBuySupplyGiftClose;
				if (onBuySupplyGiftClose == null)
				{
					return;
				}
				onBuySupplyGiftClose();
			}
		}

		public CustomImage Image_SpecialBg;

		public CustomImage Image_NormalBg;

		public CustomImage Image_Icon;

		public LoopListView2 ScrollView_Reward;

		public CustomText Text_GiftTitle;

		public CustomText Text_Title;

		public GameObject Off;

		public CustomText Text_OffValue;

		public CustomLanguageText Text_OffLabel;

		public CustomButton Button_Go;

		public CustomButton Button_Buy;

		public CustomButton Button_Price;

		public PurchaseButtonCtrl PayButtonCtrl;

		public bool IsInit;

		private ItemResources_itemget m_itemResourcesCfg;

		private ItemResources_jumpResource m_jumpResourcesCfg;

		private List<int> m_paramsIntList;

		private int m_itemResourcesType;

		private int m_paramsId;

		private PushGiftData m_giftData;

		private SupplyData m_supplyData;

		private Action<bool> m_OnBuySupplyGiftSuccess;

		private Action m_OnBuySupplyGiftClose;

		private bool _isOk;
	}
}
