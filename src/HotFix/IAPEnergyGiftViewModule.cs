using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class IAPEnergyGiftViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.buttonMask.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.giftNodeCtrl.Init();
		}

		public override void OnOpen(object data)
		{
			IAP_GiftPacks currentEnergyPack = GameApp.Data.GetDataModule(DataName.IAPDataModule).TimePackData.GetCurrentEnergyPack();
			if (currentEnergyPack == null)
			{
				this.OnClickClose();
				return;
			}
			this.giftNodeCtrl.SetData(currentEnergyPack);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.buttonMask.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			this.giftNodeCtrl.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.IAPEnergyGiftViewModule, null);
		}

		[SerializeField]
		private CustomButton buttonMask;

		[SerializeField]
		private UIEnergyGiftNodeCtrl giftNodeCtrl;
	}
}
