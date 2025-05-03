using System;
using System.Collections.Generic;
using Framework;
using Proto.Guild;

namespace HotFix
{
	public class GameEventStateGuildBoss : GameEventStateBase
	{
		public GameEventStateGuildBoss(int id)
			: base(id)
		{
		}

		public override void OnEnter()
		{
			this.mDataModule = GameApp.Data.GetDataModule(DataName.BattleGuildBossDataModule);
			AddAttributeDataModule dataModule = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
			HeroDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.HeroDataModule);
			PetDataModule dataModule3 = GameApp.Data.GetDataModule(DataName.PetDataModule);
			this.PlayerData = new BattleChapterPlayerData(dataModule2.MainCardData.m_memberID, dataModule.MemberAttributeData, dataModule.SkillIDs, dataModule3.GetFightPetCardData());
			this.m_isSendSaveSkill = false;
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
			openData.SourceType = (SkillBuildSourceType)GameConfig.GuildBoss_SkillSourceID;
			openData.TotalRound = GameConfig.GuildBoss_SelectRound;
			openData.RandomSkillNum = GameConfig.GuildBoss_RandomSkillNum;
			openData.SelectSkillNum = GameConfig.GuildBoss_SelectSkillNum;
			openData.OnSaveSkillAction = new Action<Dictionary<int, int[]>>(this.OnSaveSkillAction);
			GameApp.View.OpenView(ViewName.RoundSelectSkillViewModule, openData, 1, null, null);
		}

		public override void ContinueEvent()
		{
		}

		private void OnSaveSkillAction(Dictionary<int, int[]> skillDic)
		{
			if (this.m_isSendSaveSkill)
			{
				return;
			}
			List<int> list = new List<int>();
			foreach (int[] array in skillDic.Values)
			{
				if (array.Length != 0)
				{
					list.AddRange(array);
				}
			}
			GuildNetUtil.Guild.DoRequest_GuildBossEndBattle(list, delegate(bool result, GuildBossEndBattleResponse resp)
			{
				if (result)
				{
					this.m_isSendSaveSkill = true;
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_GuildBossBattle_Start, null);
					return;
				}
				if (resp != null && resp.Code == 6004)
				{
					string text = string.Format(Singleton<LanguageManager>.Instance.GetInfoByID("battle_check_fail"), Array.Empty<object>());
					string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("battle_check_ok");
					DxxTools.UI.OpenPopCommon(text, delegate(int id)
					{
						GameApp.View.CloseView(ViewName.GameEventViewModule, null);
						GameApp.View.CloseAllView(new int[] { 232, 1014, 214, 101, 102, 106 });
						GameApp.State.ActiveState(StateName.LoginState);
					}, string.Empty, infoByID, string.Empty, false, 2);
				}
			});
		}

		private BattleGuildBossDataModule mDataModule;

		private bool m_isSendSaveSkill;
	}
}
