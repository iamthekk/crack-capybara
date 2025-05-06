using System;
using System.Collections.Generic;
using Framework;
using Proto.Mission;

namespace HotFix
{
	public class GameEventStateWorldBoss : GameEventStateBase
	{
		public GameEventStateWorldBoss(int id)
			: base(id)
		{
		}

		public override void OnEnter()
		{
			AddAttributeDataModule dataModule = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
			HeroDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.HeroDataModule);
			PetDataModule dataModule3 = GameApp.Data.GetDataModule(DataName.PetDataModule);
			this.mDataModule = GameApp.Data.GetDataModule(DataName.WorldBossDataModule);
			this.PlayerData = new BattleChapterPlayerData(dataModule2.MainCardData.m_memberID, dataModule.MemberAttributeData, dataModule.SkillIDs, dataModule3.GetFightPetCardData());
			this.skillBuildPool = new GameEventSkillBuildFactory();
			this.skillBuildPool.Init(100);
		}

		public override void OnUpdate(float deltaTime)
		{
		}

		public override void OnExit()
		{
			this.PlayerData.ClearData();
			this.PlayerData = null;
			this.skillBuildPool = null;
		}

		public override void StartEvent()
		{
			RoundSelectSkillViewModule.OpenData openData = new RoundSelectSkillViewModule.OpenData();
			openData.Seed = this.mDataModule.BattleSkillSeed;
			openData.SourceType = (SkillBuildSourceType)GameConfig.WorldBoss_SkillSourceID;
			openData.TotalRound = GameConfig.WorldBoss_SelectRound;
			openData.RandomSkillNum = GameConfig.WorldBoss_RandomSkillNum;
			openData.SelectSkillNum = GameConfig.WorldBoss_SelectSkillNum;
			openData.OnSaveSkillAction = new Action<Dictionary<int, int[]>>(this.OnSaveSkillAction);
			GameApp.View.OpenView(ViewName.RoundSelectSkillViewModule, openData, 1, null, null);
		}

		public override void ContinueEvent()
		{
		}

		private void OnSaveSkillAction(Dictionary<int, int[]> skillDic)
		{
			List<int> list = new List<int>();
			foreach (int[] array in skillDic.Values)
			{
				if (array.Length != 0)
				{
					list.AddRange(array);
				}
			}
			NetworkUtils.WorldBoss.DoEndWorldBoss(list, delegate(bool result, EndWorldBossResponse response)
			{
				if (result)
				{
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_WorldBossBattle_Start, null);
					return;
				}
				if (response != null && response.Code == 6004)
				{
					string text = string.Format(Singleton<LanguageManager>.Instance.GetInfoByID("battle_check_fail"), Array.Empty<object>());
					string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("battle_check_ok");
					DxxTools.UI.OpenPopCommon(text, delegate(int id)
					{
						GameApp.View.CloseView(ViewName.GameEventViewModule, null);
						GameApp.View.CloseAllView(new int[] { 912, 1014, 214, 101, 102, 106 });
						GameApp.State.ActiveState(StateName.LoginState);
					}, string.Empty, infoByID, string.Empty, false, 2);
				}
			});
		}

		private WorldBossDataModule mDataModule;
	}
}
