using System;
using System.Runtime.CompilerServices;
using Framework;
using Framework.State;

namespace HotFix
{
	public class BattleDungeonState : State
	{
		public override int GetName()
		{
			return 110;
		}

		public override async void OnEnter()
		{
			await GameApp.Scene.LoadSceneAsync("Assets/_Resources/Scene/Other/Game.unity", 0, true, 100).Task;
			await GameApp.View.OpenViewTask(ViewName.BattleDungeonViewModule, null, 1, null, null);
			await GameApp.View.OpenViewTask(ViewName.BattleHUDViewModule, null, 1, null, null);
			DungeonDataModule dataModule = GameApp.Data.GetDataModule(DataName.DungeonDataModule);
			switch (dataModule.DungeonResponse.DungeonId)
			{
			case 1:
				GameApp.Sound.PlayBGM(111, 1f);
				break;
			case 2:
				GameApp.Sound.PlayBGM(112, 1f);
				break;
			case 3:
				GameApp.Sound.PlayBGM(113, 1f);
				break;
			case 4:
				GameApp.Sound.PlayBGM(114, 1f);
				break;
			}
			this.m_battleController = new BattleController_Dungeon();
			this.m_battleController.SetData((DungeonID)dataModule.DungeonResponse.DungeonId);
			await this.m_battleController.Init();
			if (GameApp.View.IsOpened(ViewName.LoadingBattleViewModule))
			{
				GameApp.View.GetViewModule(ViewName.LoadingBattleViewModule).PlayHide(delegate
				{
					GameApp.View.CloseView(ViewName.LoadingBattleViewModule, null);
					this.<OnEnter>g__OpenFight|2_1();
				});
			}
		}

		public override void OnExit()
		{
			if (GameApp.View.IsOpened(ViewName.BattleHUDViewModule))
			{
				GameApp.View.CloseView(ViewName.BattleHUDViewModule, null);
			}
			if (GameApp.View.IsOpened(ViewName.BattleDungeonViewModule))
			{
				GameApp.View.CloseView(ViewName.BattleDungeonViewModule, null);
			}
			this.m_battleController.DeInit();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			BattleController_Dungeon battleController = this.m_battleController;
			if (battleController == null)
			{
				return;
			}
			battleController.Update(deltaTime, unscaledDeltaTime);
		}

		[CompilerGenerated]
		private void <OnEnter>g__OpenFight|2_1()
		{
			BattleFightViewModule.OpenData openData = new BattleFightViewModule.OpenData();
			openData.aniFinish = delegate
			{
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_BattleStart, null);
			};
			openData.spinOffsetY = -350f;
			GameApp.View.OpenView(ViewName.BattleFightViewModule, openData, 1, null, null);
		}

		private BattleController_Dungeon m_battleController;
	}
}
