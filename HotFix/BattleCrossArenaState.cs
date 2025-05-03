using System;
using Framework;
using Framework.State;
using Framework.ViewModule;

namespace HotFix
{
	public class BattleCrossArenaState : State
	{
		public override int GetName()
		{
			return 105;
		}

		public override async void OnEnter()
		{
			GameApp.Data.GetDataModule(DataName.BattleCrossArenaDataModule).BindClothesData();
			GameApp.Sound.PlayBGM(116, 1f);
			await GameApp.Scene.LoadSceneAsync("Assets/_Resources/Scene/Other/Game.unity", 0, true, 100).Task;
			await GameApp.View.OpenViewTask(ViewName.BattleHUDViewModule, null, 1, null, null);
			await GameApp.View.OpenViewTask(ViewName.BattleCrossArenaViewModule, null, 1, null, null);
			this.m_battleController = new BattleController_CrossArena();
			await this.m_battleController.Init();
			if (GameApp.View.IsOpened(ViewName.LoadingBattleViewModule))
			{
				GameApp.View.GetViewModule(ViewName.LoadingBattleViewModule).PlayHide(delegate
				{
					GameApp.View.CloseView(ViewName.LoadingBattleViewModule, null);
					this.OpenFight();
				});
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			BattleController_CrossArena battleController = this.m_battleController;
			if (battleController == null)
			{
				return;
			}
			battleController.Update(deltaTime, unscaledDeltaTime);
		}

		public override void OnExit()
		{
			GameApp.Data.GetDataModule(DataName.BattleCrossArenaDataModule).UnBindClothesData();
			ViewModuleManager view = GameApp.View;
			if (view.IsOpened(ViewName.BattleHUDViewModule))
			{
				view.CloseView(ViewName.BattleHUDViewModule, null);
			}
			if (view.IsOpened(ViewName.BattleCrossArenaViewModule))
			{
				view.CloseView(ViewName.BattleCrossArenaViewModule, null);
			}
			this.m_battleController.DeInit();
		}

		private void OpenFight()
		{
			BattleFightViewModule.OpenData openData = new BattleFightViewModule.OpenData();
			openData.aniFinish = delegate
			{
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_BattleStart, null);
			};
			openData.spinOffsetY = -350f;
			GameApp.View.OpenView(ViewName.BattleFightViewModule, openData, 1, null, null);
		}

		private BattleController_CrossArena m_battleController;
	}
}
