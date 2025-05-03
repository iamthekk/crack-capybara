using System;
using Framework;
using Framework.State;

namespace HotFix
{
	public class BattleGuildBossState : State
	{
		public override int GetName()
		{
			return 108;
		}

		public override async void OnEnter()
		{
			await GameApp.Scene.LoadSceneAsync("Assets/_Resources/Scene/Other/Game.unity", 0, true, 100).Task;
			Singleton<GameEventController>.Instance.EnterEventMode(GameEventStateName.GuildBoss);
			await GameApp.View.OpenViewTask(ViewName.BattleGuildBossViewModule, null, 1, null, null);
			await GameApp.View.OpenViewTask(ViewName.BattleHUDViewModule, null, 1, null, null);
			this.m_battleController = new BattleController_GuildBoss();
			await this.m_battleController.Init();
			Singleton<GameEventController>.Instance.StartEvent();
			if (GameApp.View.IsOpened(ViewName.LoadingBattleViewModule))
			{
				GameApp.View.GetViewModule(ViewName.LoadingBattleViewModule).PlayHide(delegate
				{
					GameApp.View.CloseView(ViewName.LoadingBattleViewModule, null);
				});
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			BattleController_GuildBoss battleController = this.m_battleController;
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
			if (GameApp.View.IsOpened(ViewName.BattleGuildBossViewModule))
			{
				GameApp.View.CloseView(ViewName.BattleGuildBossViewModule, null);
			}
			Singleton<GameEventController>.Instance.EnterEventMode(GameEventStateName.Idle);
			this.m_battleController.DeInit();
		}

		private BattleController_GuildBoss m_battleController;
	}
}
