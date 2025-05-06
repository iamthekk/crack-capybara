using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.ViewModule;
using UnityEngine;

namespace HotFix
{
	public class PetViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			PetViewModule.PetUIPanelRT = base.GetComponent<RectTransform>();
			this.petListPage.Init();
			this.petRanchPage.Init();
			this.moduleCurrencyCtrl.Init();
			this.moduleCurrencyCtrl.SetStyle(EModuleId.PetSystem, new List<int> { 1, 2, 11 });
			this.helpButton.Init();
		}

		public override void OnOpen(object data)
		{
			this.openData = data as PetViewModule.OpenData;
			PetViewModule.OpenData openData = this.openData;
			if (((openData != null) ? openData.pageType : PetPageType.PetRanch) == PetPageType.PetRanch)
			{
				this.OnOpenPetRanchPage();
				return;
			}
			this.OpenPetListPage();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			if (this.openData != null)
			{
				Action onCloseCallback = this.openData.onCloseCallback;
				if (onCloseCallback != null)
				{
					onCloseCallback();
				}
				this.openData = null;
			}
		}

		public override void OnDelete()
		{
			this.moduleCurrencyCtrl.DeInit();
			this.petListPage.DeInit();
			this.petRanchPage.DeInit();
			this.helpButton.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_PetDataModule_UpdatePetData, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_PetDataModule_UpdatePetDrawData, new HandlerEvent(this.OnEventPetTrainingDataChange));
			GameApp.Event.RegisterEvent(LocalMessageName.SC_IAPBuySupplyGiftSuccess, new HandlerEvent(this.OnEventRefreshUI));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_PetDataModule_UpdatePetData, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_PetDataModule_UpdatePetDrawData, new HandlerEvent(this.OnEventPetTrainingDataChange));
			GameApp.Event.UnRegisterEvent(LocalMessageName.SC_IAPBuySupplyGiftSuccess, new HandlerEvent(this.OnEventRefreshUI));
		}

		public void OnClosePetListPage()
		{
			this.petListPage.OnHide();
			this.petRanchPage.OnShow();
		}

		public void OnClosePetRanchPage()
		{
			GameApp.View.CloseView(ViewName.PetViewModule, null);
		}

		public void OpenPetListPage()
		{
			this.petRanchPage.OnHide();
			this.petListPage.OnShow();
		}

		public void OnOpenPetRanchPage()
		{
			this.petRanchPage.OnShow();
			this.petListPage.OnHide();
		}

		private void OnEventRefreshUI(object sender, int type, BaseEventArgs eventArgs)
		{
			if (this.petListPage.gameObject.activeSelf)
			{
				this.petListPage.UpdateView();
			}
			this.petRanchPage.UpdateBtnView();
		}

		private void OnEventPetTrainingDataChange(object sender, int type, BaseEventArgs eventArgs)
		{
			this.petRanchPage.UpdateBtnView();
		}

		public ModuleCurrencyCtrl moduleCurrencyCtrl;

		public UIHelpButton helpButton;

		public PetListPage petListPage;

		public PetRanchPage petRanchPage;

		public static RectTransform PetUIPanelRT;

		private PetViewModule.OpenData openData;

		public class OpenData
		{
			public PetPageType pageType;

			public Action onCloseCallback;
		}
	}
}
