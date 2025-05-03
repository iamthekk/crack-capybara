using System;
using Framework;
using Framework.State;
using UnityEngine;

namespace HotFix
{
	public class BattleRogueDungeonState : State
	{
		public override int GetName()
		{
			return 111;
		}

		public override async void OnEnter()
		{
			await GameApp.Scene.LoadSceneAsync("Assets/_Resources/Scene/Other/Game.unity", 0, true, 100).Task;
			Singleton<GameEventController>.Instance.EnterEventMode(GameEventStateName.RogueDungeon);
			await GameApp.View.OpenViewTask(ViewName.BattleHUDViewModule, null, 1, null, delegate(GameObject o)
			{
				BattleHUDViewModule viewModule = GameApp.View.GetViewModule(ViewName.BattleHUDViewModule);
				if (viewModule != null)
				{
					viewModule.HideRoundText();
				}
			});
			await GameApp.View.OpenViewTask(ViewName.BattleRogueDungeonViewModule, null, 0, null, null);
			this.m_battleController = new BattleController_RogueDungeon();
			await this.m_battleController.Init();
			Singleton<GameEventController>.Instance.StartEvent();
			if (GameApp.View.IsOpened(ViewName.LoadingBattleViewModule))
			{
				GameApp.View.GetViewModule(ViewName.LoadingBattleViewModule).PlayHide(delegate
				{
					GameApp.View.CloseView(ViewName.LoadingBattleViewModule, null);
				});
			}
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_LoginViewModule_LoginAllready, null);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			BattleController_RogueDungeon battleController = this.m_battleController;
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
			if (GameApp.View.IsOpened(ViewName.BattleRogueDungeonViewModule))
			{
				GameApp.View.CloseView(ViewName.BattleRogueDungeonViewModule, null);
			}
			Singleton<GameEventController>.Instance.EnterEventMode(GameEventStateName.Idle);
			this.m_battleController.DeInit();
		}

		private BattleController_RogueDungeon m_battleController;
	}
}
