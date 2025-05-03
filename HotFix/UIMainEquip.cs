using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace HotFix
{
	public class UIMainEquip : UIBaseMainPageNode
	{
		protected override void OnInit()
		{
			this.m_mainEquipMainPanel.Init();
			this.currencyCtrl.Init();
			this.currencyCtrl.SetStyle(EModuleId.Equip, new List<int> { 1, 2, 9 });
		}

		protected override void OnShow(UIBaseMainPageNode.OpenData openData)
		{
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Main_ShowCurrency, Singleton<EventArgsBool>.Instance.SetData(true));
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Main_ShowCorner, Singleton<EventArgsBool>.Instance.SetData(false));
			this.m_mainEquipMainPanel.OnShow();
			this.currencyCtrl.SetFlyPosition();
		}

		protected override void OnHide()
		{
			this.m_mainEquipMainPanel.OnHide();
		}

		protected override void OnDeInit()
		{
			this.m_mainEquipMainPanel.DeInit();
			this.currencyCtrl.DeInit();
		}

		public override void OnLanguageChange()
		{
			this.m_mainEquipMainPanel.OnLanguageChange();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			this.currencyCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		[SerializeField]
		private UIMainMainEquipMainEquip m_mainEquipMainPanel;

		[SerializeField]
		private ModuleCurrencyCtrl currencyCtrl;
	}
}
