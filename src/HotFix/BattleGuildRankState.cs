using System;
using Framework;
using Framework.State;
using UnityEngine;

namespace HotFix
{
	public class BattleGuildRankState : State
	{
		public override int GetName()
		{
			return 107;
		}

		public override async void OnEnter()
		{
			await GameApp.Scene.LoadSceneAsync("Assets/_Resources/Scene/Other/Game.unity", 0, true, 100).Task;
			await GameApp.View.OpenViewTask(ViewName.BattleHUDViewModule, null, 1, null, null);
			await GameApp.View.OpenViewTask(ViewName.BattleGuildRankViewModule, null, 1, null, null);
			this.m_battleController = new BattleController_GuildRank();
			await this.m_battleController.Init();
			if (GameApp.View.IsOpened(ViewName.LoadingBattleViewModule))
			{
				GameApp.View.GetViewModule(ViewName.LoadingBattleViewModule).PlayHide(delegate
				{
					GameApp.View.CloseView(ViewName.LoadingBattleViewModule, null);
					GameApp.View.OpenView(ViewName.BattleFightViewModule, null, 1, null, delegate(GameObject go)
					{
						GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_BattleStart, null);
					});
				});
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			BattleController_GuildRank battleController = this.m_battleController;
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
			if (GameApp.View.IsOpened(ViewName.BattleGuildRankViewModule))
			{
				GameApp.View.CloseView(ViewName.BattleGuildRankViewModule, null);
			}
			this.m_battleController.DeInit();
		}

		private BattleController_GuildRank m_battleController;
	}
}
