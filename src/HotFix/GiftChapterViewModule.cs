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
	public class GiftChapterViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
		}

		public override void OnOpen(object data)
		{
			if (this.m_maskBt != null)
			{
				this.m_maskBt.onClick.AddListener(new UnityAction(this.CloseSelf));
			}
			if (this.m_closeBt != null)
			{
				this.m_closeBt.onClick.AddListener(new UnityAction(this.CloseSelf));
			}
			this.m_openData = data as GiftChapterViewModule.OpenData;
			if (this.m_openData == null)
			{
				HLog.LogError("GiftChapterViewModule OnOpen m_openData is null,type is OpenData");
				return;
			}
			PurchaseCommonData.PurchaseData purchaseData = this.m_openData.m_giftData.GetPurchaseData();
			if (purchaseData == null)
			{
				HLog.LogError("GiftChapterViewModule GetDataID is null");
				return;
			}
			this.m_purchaseID = purchaseData.m_id;
			IAP_PushPacks elementById = GameApp.Table.GetManager().GetIAP_PushPacksModelInstance().GetElementById(purchaseData.m_id);
			if (elementById == null)
			{
				HLog.LogError(string.Format("GiftChapterViewModule pushTable is null,id ={0}", purchaseData.m_id));
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
				this.m_maskBt.onClick.RemoveListener(new UnityAction(this.CloseSelf));
			}
			if (this.m_closeBt != null)
			{
				this.m_closeBt.onClick.RemoveListener(new UnityAction(this.CloseSelf));
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
			if (this.m_openData.m_giftData.GetLastTime() <= 0L)
			{
				NetworkUtils.PlayerData.TipSendUserGetInfoRequest("giftchapter_data_refresh", null);
				return IAPShopTimeCtrl.State.Load;
			}
			return IAPShopTimeCtrl.State.Show;
		}

		private string OnRefreshTimeText(IAPShopTimeCtrl arg)
		{
			string lastTimeString = this.m_openData.m_giftData.GetLastTimeString();
			return Singleton<LanguageManager>.Instance.GetInfoByID_LogError(3401, new object[] { lastTimeString });
		}

		private void OnEventIAPInfoData(object sender, int type, BaseEventArgs eventargs)
		{
			if (this.m_openData == null || this.m_openData.m_giftData == null)
			{
				return;
			}
			if (this.m_openData.m_giftData.IsEnable())
			{
				return;
			}
			PurchaseCommonData.PurchaseData purchaseData = this.m_openData.m_giftData.GetPurchaseData();
			if (purchaseData != null && purchaseData.m_id == this.m_purchaseID)
			{
				return;
			}
			this.CloseSelf();
		}

		private void CloseSelf()
		{
			GameApp.View.CloseView(ViewName.GiftChapterViewModule, null);
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

		private IAP_Purchase m_purchaseTable;

		private int m_purchaseID;

		private GiftChapterViewModule.OpenData m_openData;

		public class OpenData
		{
			public IAPChapterGift.Data m_giftData;
		}
	}
}
