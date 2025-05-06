using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class MiningBuyCardViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.treasureModeCtrl.Init();
			this.nextModeCtrl.Init();
			this.nextModeCtrl.SetSelect(false);
			this.treasureModeCtrl.SetSelect(true);
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnClickCancel));
			this.buttonOK.onClick.AddListener(new UnityAction(this.OnClickOK));
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			int tableID = this.iapDataModule.MonthCard.GetTableID(IAPMonthCardData.CardType.AutoMining);
			string text = "uiprivilegecard_buyText";
			this.buttonBuy.SetData(tableID, text, new Action<bool>(this.OnBuyResult), null);
			this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("uimining_card_title");
			this.miningDataModule = GameApp.Data.GetDataModule(DataName.MiningDataModule);
		}

		public override void OnOpen(object data)
		{
			this.Refresh();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.treasureModeCtrl.DeInit();
			this.nextModeCtrl.DeInit();
			this.buttonClose.onClick.RemoveListener(new UnityAction(this.OnClickCancel));
			this.buttonOK.onClick.RemoveListener(new UnityAction(this.OnClickOK));
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void Refresh()
		{
			bool flag = this.iapDataModule.MonthCard.IsActivePrivilege(CardPrivilegeKind.AutoMining);
			this.autoCardObj.SetActiveSafe(!flag);
			if (flag)
			{
				this.btnGray.Recovery();
			}
			else
			{
				this.btnGray.SetUIGray();
			}
			this.textInfo.text = Singleton<LanguageManager>.Instance.GetInfoByID("uimining_card_Info");
		}

		private void OnClickNext(bool val)
		{
			this.nextModeCtrl.SetSelect(true);
			this.treasureModeCtrl.SetSelect(false);
		}

		private void OnClickReward(bool val)
		{
			this.nextModeCtrl.SetSelect(false);
			this.treasureModeCtrl.SetSelect(true);
		}

		private void OnClickCancel()
		{
			GameApp.View.CloseView(ViewName.MiningBuyCardViewModule, null);
		}

		private void OnClickOK()
		{
			if (this.iapDataModule.MonthCard.IsActivePrivilege(CardPrivilegeKind.AutoMining))
			{
				this.OnClickCancel();
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_Mining_Auto, null);
			}
		}

		private void OnBuyResult(bool isSuccess)
		{
			this.Refresh();
			this.textInfo.text = Singleton<LanguageManager>.Instance.GetInfoByID("uimining_card_mining");
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_Mining_RefreshInfo, null);
		}

		public CustomText textTitle;

		public UICheckBoxCtrl treasureModeCtrl;

		public UICheckBoxCtrl nextModeCtrl;

		public CustomButton buttonOK;

		public CustomButton buttonClose;

		public PurchaseButtonCtrl buttonBuy;

		public GameObject autoCardObj;

		public UIGray btnGray;

		public CustomText textInfo;

		private IAPDataModule iapDataModule;

		private MiningDataModule miningDataModule;
	}
}
