using System;
using Framework;
using Framework.State;

namespace HotFix
{
	public class BattleTowerState : State
	{
		public override int GetName()
		{
			return 109;
		}

		public override async void OnEnter()
		{
			GameApp.Sound.PlayBGM(115, 1f);
			await GameApp.Scene.LoadSceneAsync("Assets/_Resources/Scene/Other/Game.unity", 0, true, 100).Task;
			await GameApp.View.OpenViewTask(ViewName.BattleTowerViewModule, null, 1, null, null);
			await GameApp.View.OpenViewTask(ViewName.BattleHUDViewModule, null, 1, null, null);
			this.m_battleController = new BattleController_Tower();
			await this.m_battleController.Init();
			if (GameApp.View.IsOpened(ViewName.LoadingBattleViewModule))
			{
				GameApp.View.GetViewModule(ViewName.LoadingBattleViewModule).PlayHide(delegate
				{
					GameApp.View.CloseView(ViewName.LoadingBattleViewModule, null);
					BattleFightViewModule.OpenData openData = new BattleFightViewModule.OpenData();
					openData.aniFinish = delegate
					{
						GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_BattleStart, null);
					};
					openData.spinOffsetY = -350f;
					GameApp.View.OpenView(ViewName.BattleFightViewModule, openData, 1, null, null);
				});
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			BattleController_Tower battleController = this.m_battleController;
			if (battleController == null)
			{
				return;
			}
			battleController.Update(deltaTime, unscaledDeltaTime);
		}

		public override void OnExit()
		{
			if (GameApp.View.IsOpened(ViewName.BattleHUDViewModule))
			{
				GameApp.View.CloseView(ViewName.BattleHUDViewModule, null);
			}
			if (GameApp.View.IsOpened(ViewName.BattleTowerViewModule))
			{
				GameApp.View.CloseView(ViewName.BattleTowerViewModule, null);
			}
			this.m_battleController.DeInit();
		}

		private BattleController_Tower m_battleController;
	}
}
