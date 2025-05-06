using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Framework.Logic;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Common;
using Proto.LeaderBoard;
using Proto.Mission;

namespace HotFix
{
	public class WorldBossDataModule : IDataModule
	{
		public int Id { get; private set; }

		public int GroupIndex
		{
			get
			{
				WorldBossDto worldBossDto = this.WorldBossDto;
				if (worldBossDto == null)
				{
					return 0;
				}
				return worldBossDto.GroupIndex;
			}
		}

		public int Round
		{
			get
			{
				WorldBossDto worldBossDto = this.WorldBossDto;
				if (worldBossDto == null)
				{
					return 0;
				}
				return worldBossDto.Round;
			}
		}

		public long NextOpenTimestamp
		{
			get
			{
				WorldBossDto worldBossDto = this.WorldBossDto;
				if (worldBossDto == null)
				{
					return 0L;
				}
				return worldBossDto.NextOpenTimestamp;
			}
		}

		public long EndTimestamp
		{
			get
			{
				WorldBossDto worldBossDto = this.WorldBossDto;
				if (worldBossDto == null)
				{
					return 0L;
				}
				return worldBossDto.EndTimestamp;
			}
		}

		public long RoundEndTimestamp
		{
			get
			{
				WorldBossDto worldBossDto = this.WorldBossDto;
				if (worldBossDto == null)
				{
					return 0L;
				}
				return worldBossDto.RoundEndTimestamp;
			}
		}

		public long RefreshTimestamp
		{
			get
			{
				WorldBossDto worldBossDto = this.WorldBossDto;
				if (worldBossDto == null)
				{
					return 0L;
				}
				return worldBossDto.RefreshTimestamp;
			}
		}

		public int ChapterId
		{
			get
			{
				WorldBossDto worldBossDto = this.WorldBossDto;
				if (worldBossDto == null)
				{
					return 0;
				}
				return worldBossDto.MissionId;
			}
		}

		public int GroupType
		{
			get
			{
				WorldBossDto worldBossDto = this.WorldBossDto;
				if (worldBossDto == null)
				{
					return 0;
				}
				return worldBossDto.GroupType;
			}
		}

		public int RankLevel
		{
			get
			{
				WorldBossDto worldBossDto = this.WorldBossDto;
				if (worldBossDto == null)
				{
					return 0;
				}
				return worldBossDto.RankLvl;
			}
		}

		public int LastRank
		{
			get
			{
				WorldBossDto worldBossDto = this.WorldBossDto;
				if (worldBossDto == null)
				{
					return 0;
				}
				return worldBossDto.LastRank;
			}
		}

		public int LastRankLevel
		{
			get
			{
				WorldBossDto worldBossDto = this.WorldBossDto;
				if (worldBossDto == null)
				{
					return 0;
				}
				return worldBossDto.LastRankLvl;
			}
		}

		public long BestDamage { get; private set; }

		public long CurrentDamage { get; private set; }

		public int ChallengeCount { get; private set; }

		public int FreeCount { get; private set; }

		public int BattleSkillSeed { get; private set; }

		public EndWorldBossResponse mEndWorldBossResponse { get; private set; }

		public RepeatedField<RankUserDto> Top3RankDataList { get; private set; } = new RepeatedField<RankUserDto>();

		public int SelfRank { get; private set; }

		public long TotalDamage { get; private set; }

		public GameMember_member bossCfg { get; private set; }

		public int BossId { get; private set; }

		public int RewardMaxClaimed { get; private set; }

		public RepeatedField<RewardDto> BoxRewardDtos { get; private set; } = new RepeatedField<RewardDto>();

		public RepeatedField<BattleRecordDto> HistoryList
		{
			get
			{
				WorldBossDto worldBossDto = this.WorldBossDto;
				return ((worldBossDto != null) ? worldBossDto.Records : null) ?? new RepeatedField<BattleRecordDto>();
			}
		}

		public int WorldBossChapter
		{
			get
			{
				return Utility.PlayerPrefs.GetUserInt("WorldBoss_WorldBossChapter", 0);
			}
			set
			{
				Utility.PlayerPrefs.SetUserInt("WorldBoss_WorldBossChapter", value);
			}
		}

		public int WorldBossTag
		{
			get
			{
				return Utility.PlayerPrefs.GetUserInt("WorldBoss_WorldBossTag", 0);
			}
			set
			{
				Utility.PlayerPrefs.SetUserInt("WorldBoss_WorldBossTag", value);
			}
		}

		public int WorldBossRound
		{
			get
			{
				return Utility.PlayerPrefs.GetUserInt("WorldBoss_WorldBossRound", 0);
			}
			set
			{
				Utility.PlayerPrefs.SetUserInt("WorldBoss_WorldBossRound", value);
			}
		}

		public int WorldBossGroupType
		{
			get
			{
				return Utility.PlayerPrefs.GetUserInt("WorldBoss_WorldBossGroupType", 0);
			}
			set
			{
				Utility.PlayerPrefs.SetUserInt("WorldBoss_WorldBossGroupType", value);
			}
		}

		public void DebugLog()
		{
		}

		public int GetName()
		{
			return 142;
		}

		public long GetRefreshCountRemainTime()
		{
			long num = this.RefreshTimestamp - DxxTools.Time.ServerTimestamp;
			if (num <= 0L)
			{
				num = 0L;
			}
			return num;
		}

		public long GetRoundRemainTime()
		{
			long num = this.RoundEndTimestamp - DxxTools.Time.ServerTimestamp;
			if (num <= 0L)
			{
				num = 0L;
			}
			return num;
		}

		public long GetSeasonRemainTime()
		{
			long num = this.EndTimestamp - DxxTools.Time.ServerTimestamp;
			if (num <= 0L)
			{
				num = 0L;
			}
			return num;
		}

		public long GetNextSeasonOpenRemainTime()
		{
			long num = this.NextOpenTimestamp - DxxTools.Time.ServerTimestamp;
			if (num <= 0L)
			{
				num = 0L;
			}
			return num;
		}

		public bool CanFreePlay()
		{
			return this.FreeCount > 0;
		}

		public bool CanPlay()
		{
			return this.GetRoundRemainTime() > 600L;
		}

		public void UpdateByServerInfo(WorldBossDto worldBossInfo)
		{
			this.netGetting = false;
			if (worldBossInfo == null || worldBossInfo.Id <= 0)
			{
				this.Id = 0;
				GlobalUpdater.Instance.UnRegisterUpdater(new Action(this.OnUpdateCheckRemainTimes));
				GameApp.Event.DispatchNow(null, LocalMessageName.CC_WorldBoss_Update, null);
				RedPointController.Instance.ReCalc("DailyActivity.ChallengeTag.WorldBoss", true);
				return;
			}
			int id = this.Id;
			int chapterId = this.ChapterId;
			int round = this.Round;
			this.WorldBossDto = worldBossInfo;
			this.Id = worldBossInfo.Id;
			this.ChallengeCount = worldBossInfo.ChallengeCnt;
			this.FreeCount = worldBossInfo.FreeCount;
			bool flag = false;
			if (id != this.Id)
			{
				flag = true;
				this.UpdateBossData();
			}
			if (chapterId != this.ChapterId)
			{
				flag = true;
				this.UpdateBossInfo();
			}
			if (flag || round != this.Round)
			{
				GameApp.Data.GetDataModule(DataName.RankDataModule).SetLastLoadedUtc(RankType.WorldBoss, 0L);
			}
			if (this.HistoryList.Count > 0)
			{
				RepeatedField<BattleRecordDto> historyList = this.HistoryList;
				this.CurrentDamage = historyList[historyList.Count - 1].Damage;
				this.BestDamage = this.CurrentDamage;
				using (IEnumerator<BattleRecordDto> enumerator = this.HistoryList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BattleRecordDto battleRecordDto = enumerator.Current;
						long damage = battleRecordDto.Damage;
						if (damage > this.BestDamage)
						{
							this.BestDamage = damage;
						}
					}
					goto IL_0160;
				}
			}
			this.CurrentDamage = 0L;
			this.BestDamage = 0L;
			IL_0160:
			this.RewardMaxClaimed = worldBossInfo.BoxRewardId;
			this.RefreshCurReward();
			if (this.Id != this.WorldBossTag || this.Round != this.WorldBossRound)
			{
				int num;
				if (this.LastRank == 0 || this.LastRankLevel == 0 || id == 0 || this.Id != id)
				{
					num = 1;
				}
				else if (this.RankLevel > this.LastRankLevel)
				{
					num = 3;
				}
				else if (this.RankLevel < this.LastRankLevel)
				{
					num = 4;
				}
				else
				{
					num = 2;
				}
				if (this.WorldBossGroupType != num)
				{
					this.WorldBossGroupType = num;
				}
				this.WorldBossRound = this.Round;
				this.WorldBossTag = this.Id;
			}
			this.SaveTop3Rank(this.WorldBossDto.RankList);
			this.SelfRank = this.WorldBossDto.SelfRank;
			this.TotalDamage = this.WorldBossDto.Damage;
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_WorldBoss_Update, null);
			RedPointController.Instance.ReCalc("DailyActivity.ChallengeTag.WorldBoss", true);
			GlobalUpdater.Instance.UnRegisterUpdater(new Action(this.OnUpdateCheckRemainTimes));
			GlobalUpdater.Instance.RegisterUpdater(new Action(this.OnUpdateCheckRemainTimes));
		}

		public void UpdateChallengeInfo(EndWorldBossResponse resp)
		{
			this.mEndWorldBossResponse = resp;
			this.UpdateByServerInfo(resp.WorldBossInfo);
		}

		public void UpdateRankData(LeaderBoardResponse resp)
		{
			if (resp.Top3.Count > 0)
			{
				this.SaveTop3Rank(resp.Top3);
			}
			this.SelfRank = resp.Self.Rank;
			this.TotalDamage = resp.Self.Score;
		}

		public void UpdateReadyBattleInfo(StartWorldBossResponse resp)
		{
			if (resp == null)
			{
				return;
			}
			this.BattleSkillSeed = resp.Seed;
			this.ChallengeCount = resp.ChallengeCnt;
			this.FreeCount = resp.FreeCount;
			RedPointController.Instance.ReCalc("DailyActivity.ChallengeTag.WorldBoss", true);
		}

		private void SaveTop3Rank(RepeatedField<RankUserDto> top3RankDataList)
		{
			this.Top3RankDataList.Clear();
			this.Top3RankDataList.AddRange(top3RankDataList);
		}

		public void UpdateRewardMaxClaimed(WorldBossBoxRewardResponse resp)
		{
			this.RewardMaxClaimed = resp.BoxRewardId;
			this.BoxRewardDtos = resp.CommonData.Reward;
			this.RefreshCurReward();
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_WorldBoss_Update, null);
			RedPointController.Instance.ReCalc("DailyActivity.ChallengeTag.WorldBoss", true);
		}

		public bool CanOpenBox()
		{
			return this.ChallengeCount > 0 && this._curRewardNeedCount <= this.ChallengeCount && !this.HasGetAllBox;
		}

		public int CurRewardNeedCount
		{
			get
			{
				return this._curRewardNeedCount;
			}
		}

		public WorldBoss_Reward CurReward
		{
			get
			{
				return this._curReward;
			}
		}

		public bool HasGetAllBox
		{
			get
			{
				return this._hasGetAllBox;
			}
		}

		private void RefreshCurReward()
		{
			this._hasGetAllBox = false;
			int count = this._bossRewards.Count;
			if (this.RewardMaxClaimed != 0)
			{
				for (int i = 0; i < count; i++)
				{
					WorldBoss_Reward worldBoss_Reward = this._bossRewards[i];
					if (worldBoss_Reward.ID == this.RewardMaxClaimed)
					{
						this._curReward = worldBoss_Reward;
						this._curRewardNeedCount = worldBoss_Reward.Times;
						if (i < count - 1)
						{
							this._curReward = this._bossRewards[i + 1];
							this._curRewardNeedCount = this._curReward.Times;
						}
						else
						{
							this._hasGetAllBox = true;
						}
						this.RewardMaxClaimed = this._curReward.ID;
						return;
					}
				}
				return;
			}
			if (count > 0)
			{
				this._curReward = this._bossRewards[0];
				this._curRewardNeedCount = this._curReward.Times;
				this.RewardMaxClaimed = this._curReward.ID;
				return;
			}
			this._curReward = null;
			this._curRewardNeedCount = 0;
			this._hasGetAllBox = true;
		}

		private void UpdateBossData()
		{
			this._bossRewards.Clear();
			IList<WorldBoss_Reward> worldBoss_RewardElements = GameApp.Table.GetManager().GetWorldBoss_RewardElements();
			bool flag = false;
			int count = worldBoss_RewardElements.Count;
			for (int i = 0; i < count; i++)
			{
				WorldBoss_Reward worldBoss_Reward = worldBoss_RewardElements[i];
				if (worldBoss_Reward.Tag == this.Id)
				{
					flag = true;
					this._bossRewards.Add(worldBoss_Reward);
				}
				else if (flag)
				{
					break;
				}
			}
		}

		private void UpdateBossInfo()
		{
			WorldBoss_WorldBoss worldBoss_WorldBoss = GameApp.Table.GetManager().GetWorldBoss_WorldBoss(this.ChapterId);
			if (worldBoss_WorldBoss != null)
			{
				this.BossId = worldBoss_WorldBoss.bossId;
				this.bossCfg = GameApp.Table.GetManager().GetGameMember_member(this.BossId);
				return;
			}
			this.BossId = 0;
			this.bossCfg = null;
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

		private void OnUpdateCheckRemainTimes()
		{
			if (!this.HasInfo())
			{
				return;
			}
			if (this.GetSeasonRemainTime() <= 0L || this.GetRoundRemainTime() <= 0L || (this.FreeCount < 1 && this.GetRefreshCountRemainTime() <= 0L))
			{
				this.GetWorldBossInfo(true, new Action<bool, int>(this.OnNet_GetWorldBossInfo));
			}
		}

		public void GetWorldBossInfo(bool isShowMask = true, Action<bool, int> callback = null)
		{
			if (this.netGetting)
			{
				return;
			}
			this.netGetting = true;
			NetworkUtils.WorldBoss.DoGetWorldBossInfo(isShowMask, callback);
		}

		private void OnNet_GetWorldBossInfo(bool result, int errorCode)
		{
			this.netGetting = false;
		}

		public List<RankReward> RefreshRoundReward(ref int myRankIndex)
		{
			bool flag = this.Round >= 4;
			if (this._lasCacheId != this.Id || this._lastCacheRankLevel != this.RankLevel || this._cacheUseLastRound != flag)
			{
				this._lasCacheId = this.Id;
				this._lastCacheRankLevel = this.RankLevel;
				this._cacheUseLastRound = flag;
				this._curRankRewards.Clear();
				IList<WorldBoss_RankReward> worldBoss_RankRewardElements = GameApp.Table.GetManager().GetWorldBoss_RankRewardElements();
				int count = worldBoss_RankRewardElements.Count;
				bool flag2 = false;
				bool flag3 = false;
				for (int i = 0; i < count; i++)
				{
					WorldBoss_RankReward worldBoss_RankReward = worldBoss_RankRewardElements[i];
					bool flag4 = worldBoss_RankReward.Tag == this.Id;
					if (!flag2 && flag4)
					{
						flag2 = true;
					}
					if (flag2 && !flag4)
					{
						break;
					}
					if (flag2)
					{
						bool flag5 = worldBoss_RankReward.SubsectionID == this.RankLevel;
						if (!flag3 && flag5)
						{
							flag3 = true;
						}
						if (flag3 && !flag5)
						{
							break;
						}
						if (flag3)
						{
							RankReward rankReward = new RankReward
							{
								RankStart = worldBoss_RankReward.RankRange[0],
								RankEnd = worldBoss_RankReward.RankRange[1]
							};
							string[] array = worldBoss_RankReward.RoundReward;
							if (flag)
							{
								array = worldBoss_RankReward.SeasonReward;
							}
							int num = array.Length;
							rankReward.Data = new PropData[num];
							for (int j = 0; j < num; j++)
							{
								string[] array2 = array[j].Split(',', StringSplitOptions.None);
								ItemData itemData = new ItemData(int.Parse(array2[0]), long.Parse(array2[1]));
								rankReward.Data[j] = itemData.ToPropData();
							}
							this._curRankRewards.Add(rankReward);
						}
					}
				}
			}
			int count2 = this._curRankRewards.Count;
			for (int k = 0; k < count2; k++)
			{
				RankReward rankReward2 = this._curRankRewards[k];
				if (rankReward2.RankStart <= this.SelfRank && this.SelfRank <= rankReward2.RankEnd)
				{
					myRankIndex = k;
					break;
				}
			}
			return this._curRankRewards;
		}

		public bool HasInfo()
		{
			return this.Id > 0;
		}

		private bool CanShow()
		{
			return this.IsOpen() && this.HasInfo();
		}

		private bool IsOpen()
		{
			return Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.WorldBoss, false);
		}

		public bool ShowAnyRed()
		{
			return this.CanShow() && (this.CanOpenBox() || (this.GetSeasonRemainTime() > 0L && this.CanPlay() && this.FreeCount > 0));
		}

		public void ShowNextSeasonTimeTip()
		{
			long nextSeasonOpenRemainTime = this.GetNextSeasonOpenRemainTime();
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("WorldBoss_NextBattle_AfterTime", new object[] { Singleton<LanguageManager>.Instance.GetTime(nextSeasonOpenRemainTime) });
			GameApp.View.ShowStringTip(infoByID);
		}

		public void ShowNoSeasonTip()
		{
			GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("WorldBoss_GetInfo_SysEnd"));
		}

		public static string GetRankLevelShow(int rankLevel)
		{
			WorldBoss_Subsection worldBoss_Subsection = GameApp.Table.GetManager().GetWorldBoss_Subsection(rankLevel);
			return Singleton<LanguageManager>.Instance.GetInfoByID(worldBoss_Subsection.languageId);
		}

		private WorldBossDto WorldBossDto;

		private readonly List<WorldBoss_Reward> _bossRewards = new List<WorldBoss_Reward>();

		private WorldBoss_Reward _curReward;

		private int _curRewardNeedCount;

		private bool _hasGetAllBox;

		private bool netGetting;

		private readonly List<RankReward> _curRankRewards = new List<RankReward>();

		private int _lastCacheRankLevel;

		private int _lasCacheId;

		private bool _cacheUseLastRound;
	}
}
