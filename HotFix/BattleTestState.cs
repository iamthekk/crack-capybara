using System;
using Framework;
using Framework.State;

namespace HotFix
{
	public class BattleTestState : State
	{
		public override int GetName()
		{
			return 113;
		}

		public override async void OnEnter()
		{
			await GameApp.Scene.LoadSceneAsync("Assets/_Resources/Scene/Other/Game.unity", 0, true, 100).Task;
			await GameApp.View.OpenViewTask(ViewName.BattleTestModule, null, 1, null, null);
			await GameApp.View.OpenViewTask(ViewName.BattleHUDViewModule, null, 1, null, null);
			this.m_battleController = new BattleController_Test();
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
					GameApp.View.OpenView(ViewName.BattleFightViewModule, openData, 1, null, null);
				});
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			BattleController_Test battleController = this.m_battleController;
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
			if (GameApp.View.IsOpened(ViewName.BattleTestModule))
			{
				GameApp.View.CloseView(ViewName.BattleTestModule, null);
			}
			this.m_battleController.DeInit();
		}

		private void ClothesTest()
		{
			ClothesData selfClothesData = GameApp.Data.GetDataModule(DataName.ClothesDataModule).SelfClothesData;
			if (selfClothesData != null)
			{
				foreach (SkinData skinData in selfClothesData.GetSkinDatas().Values)
				{
					HLog.LogError(string.Format("skinType = {0}  skinID = {1}  skinPrefabID = {2}  path = {3}", new object[] { skinData.SkinType, skinData.SkinID, skinData.SkinPrefabID, skinData.Path }));
				}
			}
		}

		private BattleController_Test m_battleController;
	}
}
