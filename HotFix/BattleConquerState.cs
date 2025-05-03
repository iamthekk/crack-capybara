﻿using System;
using Framework;
using Framework.State;
using UnityEngine;

namespace HotFix
{
	public class BattleConquerState : State
	{
		public override int GetName()
		{
			return 106;
		}

		public override async void OnEnter()
		{
			await GameApp.Scene.LoadSceneAsync("Assets/_Resources/Scene/Other/Game.unity", 0, true, 100).Task;
			await GameApp.View.OpenViewTask(ViewName.BattleHUDViewModule, null, 1, null, null);
			await GameApp.View.OpenViewTask(ViewName.BattleConquerViewModule, null, 1, null, null);
			this.m_battleController = new BattleController_Conquer();
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
			BattleController_Conquer battleController = this.m_battleController;
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
			if (GameApp.View.IsOpened(ViewName.BattleConquerViewModule))
			{
				GameApp.View.CloseView(ViewName.BattleConquerViewModule, null);
			}
			this.m_battleController.DeInit();
		}

		private BattleController_Conquer m_battleController;
	}
}
