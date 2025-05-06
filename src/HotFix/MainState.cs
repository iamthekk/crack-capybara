using System;
using Framework;
using Framework.State;
using UnityEngine;

namespace HotFix
{
	public class MainState : State
	{
		public static int EnterCount { get; private set; }

		public override int GetName()
		{
			return 103;
		}

		public override async void OnEnter()
		{
			MainState.EnterCount++;
			this.CheckPopEnabled = false;
			RedPointController.Instance.GotoMain();
			await GameApp.Scene.LoadSceneAsync("Assets/_Resources/Scene/Other/Main.unity", 0, true, 100).Task;
			this.hangUpManager = new HangUpManager();
			await this.hangUpManager.Init();
			this.sweepMapManager = new SweepMapManager();
			this.sweepMapManager.Init();
			await GameApp.View.OpenViewTask(ViewName.MainViewModule, null, 0, null, null);
			MainDataModule mainDataModule = GameApp.Data.GetDataModule(DataName.MainDataModule);
			if (mainDataModule.m_mainOpenData != null)
			{
				GameApp.View.GetViewModule(ViewName.MainViewModule).GotoPage(mainDataModule.m_mainOpenData.m_pageName, null);
				for (int i = 0; i < mainDataModule.m_mainOpenData.m_openViewDatas.Count; i++)
				{
					MainOpenViewData mainOpenViewData = mainDataModule.m_mainOpenData.m_openViewDatas[i];
					if (mainOpenViewData != null)
					{
						await GameApp.View.OpenViewTask((ViewName)mainOpenViewData.m_viewID, mainOpenViewData.m_openData, mainOpenViewData.m_layer, null, null);
					}
				}
				EventArgsRefreshMainOpenData instance = Singleton<EventArgsRefreshMainOpenData>.Instance;
				instance.m_openData = null;
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_MainDataModule_RefreshMainOpenData, instance);
				if (mainDataModule.m_mainOpenData != null && mainDataModule.m_mainOpenData.m_pageName == UIMainPageName.Battle)
				{
					int count = mainDataModule.m_mainOpenData.m_openViewDatas.Count;
				}
			}
			if (GameApp.View.IsOpened(ViewName.LoginViewModule))
			{
				HangUpManager.Instance.StartMove();
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_LoginViewModule_LoginAllready, null);
			}
			else if (GameApp.View.IsOpened(ViewName.LoadingMainViewModule))
			{
				GameApp.View.GetViewModule(ViewName.LoadingMainViewModule).PlayHide(delegate
				{
					GameApp.View.CloseView(ViewName.LoadingMainViewModule, null);
					HangUpManager.Instance.StartMove();
				});
			}
			else
			{
				GameApp.View.OpenView(ViewName.LoadingMainViewModule, null, 1, null, delegate(GameObject loadObj)
				{
					GameApp.View.GetViewModule(ViewName.LoadingMainViewModule).PlayHide(delegate
					{
						GameApp.View.CloseView(ViewName.LoadingMainViewModule, null);
						HangUpManager.Instance.StartMove();
					});
				});
			}
			GameApp.Sound.PlayBGM(1, 1f);
			this.m_mailHeartbeatCtrl = new HeartBeatCtrl_Mail();
			this.m_mailHeartbeatCtrl.OnInit();
			this.m_updateHeartbeatCtrl = new HeartBeatCtrl_Update();
			this.m_updateHeartbeatCtrl.OnInit();
			this.m_viewPop = new MainStateViewPopController();
			this.m_viewPop.Init();
			this.m_viewPop.OnLoadFinished();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			HeartBeatCtrl_Mail mailHeartbeatCtrl = this.m_mailHeartbeatCtrl;
			if (mailHeartbeatCtrl != null)
			{
				mailHeartbeatCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			HeartBeatCtrl_Update updateHeartbeatCtrl = this.m_updateHeartbeatCtrl;
			if (updateHeartbeatCtrl != null)
			{
				updateHeartbeatCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			MainStateViewPopController viewPop = this.m_viewPop;
			if (viewPop != null)
			{
				viewPop.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			SweepMapManager sweepMapManager = this.sweepMapManager;
			if (sweepMapManager == null)
			{
				return;
			}
			sweepMapManager.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void OnExit()
		{
			HeartBeatCtrl_Mail mailHeartbeatCtrl = this.m_mailHeartbeatCtrl;
			if (mailHeartbeatCtrl != null)
			{
				mailHeartbeatCtrl.OnDeInit();
			}
			this.m_mailHeartbeatCtrl = null;
			HeartBeatCtrl_Update updateHeartbeatCtrl = this.m_updateHeartbeatCtrl;
			if (updateHeartbeatCtrl != null)
			{
				updateHeartbeatCtrl.OnDeInit();
			}
			this.m_updateHeartbeatCtrl = null;
			MainStateViewPopController viewPop = this.m_viewPop;
			if (viewPop != null)
			{
				viewPop.DeInit();
			}
			this.m_viewPop = null;
			HangUpManager hangUpManager = this.hangUpManager;
			if (hangUpManager != null)
			{
				hangUpManager.DeInit();
			}
			SweepMapManager sweepMapManager = this.sweepMapManager;
			if (sweepMapManager != null)
			{
				sweepMapManager.DeInit();
			}
			GameApp.View.CloseAllView(new int[] { 102, 105, 101, 106, 972 });
		}

		public bool CheckPopEnabled { get; private set; }

		public void SetCheckEnabled(bool enabled)
		{
			this.CheckPopEnabled = enabled;
		}

		public static void ResetEnterCount()
		{
			MainState.EnterCount = 0;
		}

		public HeartBeatCtrl_Mail m_mailHeartbeatCtrl;

		public HeartBeatCtrl_Update m_updateHeartbeatCtrl;

		private MainStateViewPopController m_viewPop;

		private HangUpManager hangUpManager;

		private SweepMapManager sweepMapManager;
	}
}
