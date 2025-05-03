using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.State;
using UnityEngine;

namespace HotFix
{
	public class LoginState : State
	{
		public override int GetName()
		{
			return 101;
		}

		public override async void OnEnter()
		{
			await Singleton<HotfixVersionMgr>.Instance.LoadVersion();
			await Singleton<BattleServerVersionMgr>.Instance.LoadVersion();
			GlobalUpdater.Instance.ClearAll();
			HeartBeatCtrl_ServerTimeClock.Instance.RemoveAll();
			foreach (KeyValuePair<int, IDataModule> keyValuePair in GameApp.Data.GetAllDataModules())
			{
				keyValuePair.Value.Reset();
			}
			GameApp.Event.DispatchNow(this, 103, null);
			await GameApp.Scene.LoadSceneAsync("Assets/_Resources/Scene/Other/Login.unity", 0, true, 100).Task;
			int mainStepIndex = LoadingTime.MainStepIndex;
			GameApp.View.OpenView(ViewName.LoginViewModule, null, 2, null, delegate(GameObject obj)
			{
				if (GameApp.View.IsOpened(ViewName.CheckAssetsViewModule))
				{
					GameApp.View.CloseView(ViewName.CheckAssetsViewModule, null);
				}
				if (GameApp.View.IsOpened(ViewName.LoadingViewModule))
				{
					GameApp.View.GetViewModule(ViewName.LoadingViewModule).PlayHide(delegate
					{
						GameApp.View.CloseView(ViewName.LoadingViewModule, null);
					});
				}
			});
			GameApp.View.OpenView(ViewName.TipViewModule, null, 2, null, null);
			GameApp.View.OpenView(ViewName.FlyItemViewModule, null, 2, null, null);
			GameApp.Sound.PlayBGM(1, 1f);
		}

		public override void OnExit()
		{
			GameApp.Sound.StopBackground();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}
	}
}
