using System;
using Framework;
using Framework.Platfrom;
using Framework.State;

namespace HotFix
{
	public class FirstEnterWorldState : State
	{
		public override int GetName()
		{
			return 102;
		}

		public override async void OnEnter()
		{
			this.StartSoundBg();
			await GameApp.Scene.LoadSceneAsync("Assets/_Resources/Scene/Other/FirstEnterWorld.unity", 0, true, 100).Task;
			GameApp.View.OpenView(ViewName.FirstEnterWorldViewModule, null, 0, null, null);
			if (GameApp.View.IsOpened(ViewName.LoadingMainViewModule))
			{
				GameApp.View.GetViewModule(ViewName.LoadingMainViewModule).PlayHide(delegate
				{
					GameApp.View.CloseView(ViewName.LoadingMainViewModule, null);
				});
			}
		}

		public override void OnExit()
		{
			this.StopSoundBg();
			if (GameApp.View.IsOpened(ViewName.FirstEnterWorldViewModule))
			{
				GameApp.View.CloseView(ViewName.FirstEnterWorldViewModule, null);
			}
			HLog.LogError("待接入引导系统，设置cg引导结束");
			PlayerPrefsExpand.SetInt("FirstEnterWorldState", 1);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		private void StartSoundBg()
		{
			GameApp.Sound.PlayBGM(3, 1f);
		}

		private void StopSoundBg()
		{
			GameApp.Sound.StopBackground();
		}
	}
}
