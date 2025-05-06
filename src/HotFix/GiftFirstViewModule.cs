using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.Pay;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class GiftFirstViewModule : BaseViewModule
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
			IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			int dataID = dataModule.FirstGift.GetDataID();
			IAP_PushPacks elementById = GameApp.Table.GetManager().GetIAP_PushPacksModelInstance().GetElementById(dataID);
			if (elementById == null)
			{
				HLog.LogError(string.Format("GiftFirstViewModule table is null,id ={0}", dataID));
				return;
			}
			bool flag = dataModule.FirstGift.IsConsume();
			if (this.m_gotoBt != null)
			{
				this.m_gotoBt.gameObject.SetActive(!flag);
			}
			if (this.m_receiveBt != null)
			{
				this.m_receiveBt.gameObject.SetActive(flag);
			}
			if (this.m_rewardCtrl != null)
			{
				List<ItemData> rewardItemData = elementById.GetRewardItemData();
				this.m_rewardCtrl.SetData(rewardItemData, true);
				this.m_rewardCtrl.Init();
				this.m_rewardCtrl.SetActiveForReceive(false);
			}
			if (this.m_gotoBt != null)
			{
				this.m_gotoBt.onClick.AddListener(new UnityAction(this.OnClickGotoBt));
			}
			if (this.m_receiveBt != null)
			{
				this.m_receiveBt.onClick.AddListener(new UnityAction(this.OnClickReceiveBt));
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
				return;
			}
			this.m_discountTxt.text = string.Empty;
			this.m_discountTxt2.text = string.Empty;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			if (this.m_maskBt != null)
			{
				this.m_maskBt.onClick.RemoveListener(new UnityAction(this.OnClickCloseBt));
			}
			if (this.m_closeBt != null)
			{
				this.m_closeBt.onClick.RemoveListener(new UnityAction(this.OnClickCloseBt));
			}
			if (this.m_gotoBt != null)
			{
				this.m_gotoBt.onClick.RemoveListener(new UnityAction(this.OnClickGotoBt));
			}
			if (this.m_receiveBt != null)
			{
				this.m_receiveBt.onClick.RemoveListener(new UnityAction(this.OnClickReceiveBt));
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

		private void OnClickCloseBt()
		{
			this.CloseSelf();
		}

		private void OnClickGotoBt()
		{
			this.CloseSelf();
			IAPShopViewModule.OpenData openData = new IAPShopViewModule.OpenData(IAPShopJumpTabData.CreateDiamonds(IAPDiamondsType.DiamondsPack));
			GameApp.View.OpenView(ViewName.IAPShopViewModule, openData, 1, null, null);
		}

		private void OnClickReceiveBt()
		{
			NetworkUtils.Purchase.SendFirstRechargeRewardRequest(delegate(bool isOk, FirstRechargeRewardResponse resp)
			{
				if (!isOk)
				{
					return;
				}
				DxxTools.UI.OpenRewardCommon(resp.CommonData.Reward, null, true);
				if (GameApp.View.IsOpened(ViewName.GiftFirstViewModule))
				{
					GameApp.View.CloseView(ViewName.GiftFirstViewModule, null);
				}
			});
		}

		private void CloseSelf()
		{
			GameApp.View.CloseView(ViewName.GiftFirstViewModule, null);
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
		private CustomButton m_gotoBt;

		[SerializeField]
		private CustomButton m_receiveBt;

		[SerializeField]
		private CustomText m_discountTxt;

		[SerializeField]
		private CustomText m_discountTxt2;
	}
}
