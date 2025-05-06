using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.ViewModule;
using UnityEngine;

namespace HotFix
{
	public class CurrencyViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			if (this.m_currencyCtrl != null)
			{
				this.m_currencyCtrl.gameObject.SetActive(true);
				this.m_currencyCtrl.Init();
			}
			if (this.miniGameCurrencyCtrl)
			{
				this.miniGameCurrencyCtrl.gameObject.SetActive(false);
			}
			this.m_currencyGetPrefab.SetActive(false);
		}

		public override void OnOpen(object data)
		{
			if (this.m_currencyCtrl)
			{
				this.m_currencyCtrl.OnRefreshAll();
			}
			GameApp.CoroutineSystem.AddTask(1, this.onUpdateCurrencyPosition());
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.m_seqPool.Clear(false);
			if (this.m_currencyCtrl != null)
			{
				this.m_currencyCtrl.DeInit();
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.m_currencyCtrl != null)
			{
				this.m_currencyCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private IEnumerator onUpdateCurrencyPosition()
		{
			yield return new WaitForEndOfFrame();
			if (this.m_currencyCtrl != null)
			{
				EventArgFlyItemViewModuleSetEnd eventArgFlyItemViewModuleSetEnd = new EventArgFlyItemViewModuleSetEnd();
				eventArgFlyItemViewModuleSetEnd.SetData(FlyItemModel.Default, CurrencyType.Gold, new List<Transform> { this.m_currencyCtrl.m_goldUI.Image.transform });
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_FlyItemViewModule_SetEnd, eventArgFlyItemViewModuleSetEnd);
				EventArgFlyItemViewModuleSetEnd eventArgFlyItemViewModuleSetEnd2 = new EventArgFlyItemViewModuleSetEnd();
				eventArgFlyItemViewModuleSetEnd2.SetData(FlyItemModel.Default, CurrencyType.Diamond, new List<Transform> { this.m_currencyCtrl.m_diamondUI.Image.transform });
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_FlyItemViewModule_SetEnd, eventArgFlyItemViewModuleSetEnd2);
				EventArgFlyItemViewModuleSetEnd eventArgFlyItemViewModuleSetEnd3 = new EventArgFlyItemViewModuleSetEnd();
				eventArgFlyItemViewModuleSetEnd3.SetData(FlyItemModel.Default, CurrencyType.TeamExp, new List<Transform> { this.m_currencyCtrl.mPlayerInfoNode.m_avatar.transform });
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_FlyItemViewModule_SetEnd, eventArgFlyItemViewModuleSetEnd3);
			}
			GameApp.CoroutineSystem.RemoveTask(1, this.onUpdateCurrencyPosition());
			yield break;
		}

		public const string CurrenctGetName = "currency_get";

		public UICurrencyCtrl m_currencyCtrl;

		public UICurrencyCtrl miniGameCurrencyCtrl;

		public GameObject m_currencyGetPrefab;

		public Transform m_currencyGetNode;

		public Transform m_currencyGetNodeSecond;

		private SequencePool m_seqPool = new SequencePool();
	}
}
