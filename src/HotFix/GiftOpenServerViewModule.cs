using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class GiftOpenServerViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
		}

		public override void OnOpen(object data)
		{
			if (this.m_maskBt != null)
			{
				this.m_maskBt.onClick.AddListener(new UnityAction(this.OnClickCloseBt));
			}
			if (this.m_closeBt != null)
			{
				this.m_closeBt.onClick.AddListener(new UnityAction(this.OnClickCloseBt));
			}
			this.m_iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			PurchaseCommonData.PurchaseData purchaseData = this.m_iapDataModule.OpenServerGift.GetPurchaseData();
			if (purchaseData == null)
			{
				HLog.LogError("GiftOpenServerViewModule GetDataID is null");
				return;
			}
			IAP_PushPacks elementById = GameApp.Table.GetManager().GetIAP_PushPacksModelInstance().GetElementById(purchaseData.m_id);
			if (elementById == null)
			{
				HLog.LogError(string.Format("GiftOpenServerViewModule pushTable is null,id ={0}", purchaseData.m_id));
				return;
			}
			if (this.m_timeCtrl != null)
			{
				this.m_timeCtrl.OnRefreshText += this.OnRefreshTimeText;
				this.m_timeCtrl.OnChangeState += this.OnChangeState;
				this.m_timeCtrl.Init();
				this.m_timeCtrl.Play();
			}
			if (this.m_rewardCtrl != null)
			{
				List<ItemData> rewardItemData = elementById.GetRewardItemData();
				this.m_rewardCtrl.SetData(rewardItemData, true);
				this.m_rewardCtrl.Init();
				this.m_rewardCtrl.SetActiveForReceive(false);
			}
			if (this.m_buyBt != null)
			{
				this.m_buyBt.SetData(purchaseData.m_id, null, null, delegate(int id)
				{
					this.CloseSelf();
				}, null, null);
			}
			if (this.m_titleText != null)
			{
				this.m_titleText.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameID);
			}
			if (this.m_descText != null)
			{
				this.m_descText.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.descID);
			}
			if (elementById.valueDescID != null && elementById.valueDescID.Length >= 2)
			{
				this.m_discountTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.valueDescID[0]);
				this.m_discountTxt2.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.valueDescID[1]);
			}
			else
			{
				this.m_discountTxt.text = string.Empty;
				this.m_discountTxt2.text = string.Empty;
			}
			GameApp.Event.RegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPInfoData, new HandlerEvent(this.OnEventIAPInfoData));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.m_timeCtrl != null)
			{
				this.m_timeCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public override void OnClose()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPInfoData, new HandlerEvent(this.OnEventIAPInfoData));
			if (this.m_timeCtrl != null)
			{
				this.m_timeCtrl.DeInit();
			}
			if (this.m_maskBt != null)
			{
				this.m_maskBt.onClick.RemoveListener(new UnityAction(this.OnClickCloseBt));
			}
			if (this.m_closeBt != null)
			{
				this.m_closeBt.onClick.RemoveListener(new UnityAction(this.OnClickCloseBt));
			}
			if (this.m_rewardCtrl != null)
			{
				this.m_rewardCtrl.DeInit();
			}
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private IAPShopTimeCtrl.State OnChangeState(IAPShopTimeCtrl arg)
		{
			if (this.m_iapDataModule.OpenServerGift.GetLastTime() <= 0L)
			{
				NetworkUtils.PlayerData.TipSendUserGetInfoRequest("giftchapter_data_refresh", null);
				return IAPShopTimeCtrl.State.Load;
			}
			return IAPShopTimeCtrl.State.Show;
		}

		private string OnRefreshTimeText(IAPShopTimeCtrl arg)
		{
			string lastTimeString = this.m_iapDataModule.OpenServerGift.GetLastTimeString();
			return Singleton<LanguageManager>.Instance.GetInfoByID_LogError(3401, new object[] { lastTimeString });
		}

		private void OnClickCloseBt()
		{
			this.CloseSelf();
		}

		private void OnEventIAPInfoData(object sender, int type, BaseEventArgs eventargs)
		{
			if (this.m_iapDataModule.OpenServerGift.IsEnable())
			{
				return;
			}
			this.OnClickCloseBt();
		}

		private void CloseSelf()
		{
			GameApp.View.CloseView(ViewName.GiftOpenServerViewModule, null);
		}

		[SerializeField]
		private CustomText m_titleText;

		[SerializeField]
		private CustomText m_descText;

		[SerializeField]
		private CustomButton m_maskBt;

		[SerializeField]
		private CustomButton m_closeBt;

		[SerializeField]
		private IAPShopRewardCtrl m_rewardCtrl;

		[SerializeField]
		private PurchaseButtonCtrl m_buyBt;

		[SerializeField]
		private CustomText m_discountTxt;

		[SerializeField]
		private CustomText m_discountTxt2;

		[SerializeField]
		private IAPShopTimeCtrl m_timeCtrl;

		private IAPDataModule m_iapDataModule;
	}
}
