using System;
using System.Collections.Generic;
using System.Text;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Battle;
using Proto.Common;
using Proto.Tower;

namespace HotFix
{
	public class RogueDungeonDataModule : IDataModule
	{
		public bool IsBattleSign { get; private set; }

		public uint ServerFloorID { get; private set; }

		public int FloorSeed { get; private set; }

		public uint PassFloorCount { get; private set; }

		public RepeatedField<int> MonsterSkills { get; private set; }

		public RepeatedField<int> MonsterCfgList { get; private set; }

		public MapField<string, int> PlayerAttMap { get; private set; }

		public long PlayerCurrentHp { get; private set; }

		public int RevertCount { get; private set; }

		public uint BattleWaveRate { get; private set; }

		public uint EventID { get; private set; }

		public RepeatedField<RewardDto> TotalReward { get; private set; }

		public int PlayerRank { get; private set; }

		public uint CurrentFloorID
		{
			get
			{
				return this.ServerFloorID / 1000U;
			}
		}

		public uint CurrentWaveIndex
		{
			get
			{
				return this.ServerFloorID % 1000U;
			}
		}

		public bool IsEnterSkill { get; private set; }

		public bool IsRoundSkill { get; private set; }

		public int BattleResult { get; private set; }

		public long BattleHP { get; private set; }

		public RHellTowerCombatReq CombatReq { get; private set; }

		public int GetName()
		{
			return 163;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public void Reset()
		{
		}

		public void SetLoginData(uint serverStage, uint isBattle)
		{
			this.ServerFloorID = serverStage;
			this.IsBattleSign = isBattle > 0U;
		}

		public void UpdateBaseInfo(HellGetPanelInfoResponse resp)
		{
			if (resp == null)
			{
				return;
			}
			this.ServerFloorID = resp.CurStage;
			this.IsBattleSign = resp.BattleStatus > 0U;
			this.FloorSeed = (int)resp.StageSeed;
			this.PassFloorCount = resp.RoundPassStage;
			this.MonsterSkills = resp.MonsterSkillList;
			this.MonsterCfgList = resp.MonsterCfgId;
			this.PlayerAttMap = resp.AttrMap;
			this.PlayerCurrentHp = resp.Hp;
			this.RevertCount = resp.RevertCount;
			this.BattleWaveRate = resp.WaveRate;
			this.EventID = resp.EventId;
			this.PlayerSkills = resp.AllSkillList;
			this.IsEnterSkill = resp.IsEnterSkill > 0U;
			this.IsRoundSkill = resp.IsRoundSkill > 0U;
		}

		public void EnterChallenge(HellEnterBattleResponse resp)
		{
			if (resp == null)
			{
				return;
			}
			this.ServerFloorID = resp.CurStage;
			this.FloorSeed = (int)resp.StageSeed;
			this.MonsterSkills = resp.MonsterSkillList;
			this.MonsterCfgList = resp.MonsterCfgId;
			this.PlayerAttMap = resp.AttrMap;
			this.BattleWaveRate = resp.WaveRate;
			this.IsBattleSign = false;
			this.PassFloorCount = 0U;
			this.PlayerSkills = null;
			this.RevertCount = 0;
			this.EventID = 0U;
			this.IsEnterSkill = false;
			this.IsRoundSkill = false;
		}

		public void SetRoundEnterSkillSelected()
		{
			this.IsEnterSkill = true;
			this.IsRoundSkill = true;
		}

		public void SetRoundEnter()
		{
			this.IsRoundSkill = false;
		}

		public void UpdateAttribute(MapField<string, int> map)
		{
			this.PlayerAttMap = map;
		}

		public void UpdateBattleInfo(HellDoChallengeResponse resp)
		{
			if (resp == null)
			{
				return;
			}
			this.BattleResult = resp.Result;
			this.BattleHP = resp.Hp;
			this.CombatReq = new RHellTowerCombatReq
			{
				StageId = this.ServerFloorID,
				PassStage = this.PassFloorCount,
				UserInfo = resp.UserInfo,
				Seed = resp.Seed,
				ReviveCount = resp.RevertCount,
				BattleServerLogId = resp.BattleServerLogId,
				BattleServerLogData = resp.BattleServerLogData
			};
			this.CombatReq.MonsterCfgId.AddRange(resp.MonsterCfgId);
			for (int i = 0; i < this.MonsterSkills.Count; i++)
			{
				this.CombatReq.MonsterSkillList.Add(this.MonsterSkills[i]);
			}
			this.ServerFloorID = resp.Stage;
			this.PassFloorCount = resp.RoundPassStage;
			this.FloorSeed = (int)resp.StageSeed;
			this.EventID = resp.EventId;
			this.MonsterCfgList = resp.MonsterCfgIdNext;
			this.TotalReward = resp.CommonData.Reward;
			if (this.TotalReward != null && this.TotalReward.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int j = 0; j < this.TotalReward.Count; j++)
				{
					stringBuilder.Append(string.Format("{0}, {1}", this.TotalReward[j].ConfigId, this.TotalReward[j].Count));
					if (j != this.TotalReward.Count - 1)
					{
						stringBuilder.Append("|");
					}
				}
			}
			this.IsBattleSign = false;
		}

		public void Escape(RepeatedField<RewardDto> rewardDtos)
		{
			this.IsBattleSign = false;
			if (rewardDtos != null && rewardDtos.Count > 0)
			{
				this.TotalReward = rewardDtos;
			}
		}

		public void UpdateRank(int page, HellRankResponse resp)
		{
			if (resp == null)
			{
				return;
			}
			this.PlayerRank = resp.RankIndex;
		}

		public List<int> GetPlayerSkillIds()
		{
			List<int> list = new List<int>();
			if (this.PlayerSkills != null)
			{
				for (int i = 0; i < this.PlayerSkills.Count; i++)
				{
					list.Add(this.PlayerSkills[i]);
				}
			}
			return list;
		}

		public RogueDungeon_rogueDungeon GetCurrentTable()
		{
			return GameApp.Table.GetManager().GetRogueDungeon_rogueDungeon((int)this.CurrentFloorID);
		}

		public bool IsBattleEndRecoverDisabled()
		{
			if (this.MonsterSkills == null)
			{
				return false;
			}
			for (int i = 0; i < this.MonsterSkills.Count; i++)
			{
				RogueDungeon_monsterEntry rogueDungeon_monsterEntry = GameApp.Table.GetManager().GetRogueDungeon_monsterEntry(this.MonsterSkills[i]);
				if (rogueDungeon_monsterEntry != null && rogueDungeon_monsterEntry.actionType == 4)
				{
					return true;
				}
			}
			return false;
		}

		private RepeatedField<int> PlayerSkills;
	}
}
