using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	[GuildInternalModule(17)]
	public class GuildActivityDataModule : IGuildModule
	{
		public int ModuleName
		{
			get
			{
				return 17;
			}
		}

		public bool Init(GuildInitConfig config)
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
			@event.RegisterEvent(10, new GuildHandlerEvent(this.OnLoginSuccess));
			@event.RegisterEvent(201, new GuildHandlerEvent(this.OnGuildBossCountChange));
			@event.RegisterEvent(202, new GuildHandlerEvent(this.OnGuildBossInfoSet));
			@event.RegisterEvent(203, new GuildHandlerEvent(this.OnGuildBossTaskSet));
			@event.RegisterEvent(204, new GuildHandlerEvent(this.OnGuildBossBoxSet));
			@event.RegisterEvent(205, new GuildHandlerEvent(this.OnGuildBossSetTransID));
			@event.RegisterEvent(206, new GuildHandlerEvent(this.OnCacheGuildUserBossRankList));
			@event.RegisterEvent(207, new GuildHandlerEvent(this.OnCacheGuildBossGuildRankList));
			@event.RegisterEvent(401, new GuildHandlerEvent(this.OnRaceSetBaseInfo));
			@event.RegisterEvent(402, new GuildHandlerEvent(this.OnRaceApply));
			@event.RegisterEvent(210, new GuildHandlerEvent(this.OnGuildBossBoxRewardChange));
			return true;
		}

		public void UnInit()
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
			@event.UnRegisterEvent(10, new GuildHandlerEvent(this.OnLoginSuccess));
			@event.UnRegisterEvent(201, new GuildHandlerEvent(this.OnGuildBossCountChange));
			@event.UnRegisterEvent(202, new GuildHandlerEvent(this.OnGuildBossInfoSet));
			@event.UnRegisterEvent(203, new GuildHandlerEvent(this.OnGuildBossTaskSet));
			@event.UnRegisterEvent(204, new GuildHandlerEvent(this.OnGuildBossBoxSet));
			@event.UnRegisterEvent(205, new GuildHandlerEvent(this.OnGuildBossSetTransID));
			@event.UnRegisterEvent(206, new GuildHandlerEvent(this.OnCacheGuildUserBossRankList));
			@event.UnRegisterEvent(207, new GuildHandlerEvent(this.OnCacheGuildBossGuildRankList));
			@event.UnRegisterEvent(401, new GuildHandlerEvent(this.OnRaceSetBaseInfo));
			@event.UnRegisterEvent(402, new GuildHandlerEvent(this.OnRaceApply));
			@event.UnRegisterEvent(210, new GuildHandlerEvent(this.OnGuildBossBoxRewardChange));
		}

		private void OnLoginSuccess(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_LoginSuccess guildEvent_LoginSuccess = eventArgs as GuildEvent_LoginSuccess;
			if (guildEvent_LoginSuccess != null)
			{
				if (guildEvent_LoginSuccess.IsJoin)
				{
					this.mGuildBossInfo = guildEvent_LoginSuccess.Boss;
					return;
				}
				this.mGuildBossInfo = new GuildBossInfo();
			}
		}

		private void OnGuildBossCountChange(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_GuildBossCount guildEvent_GuildBossCount = eventArgs as GuildEvent_GuildBossCount;
			if (guildEvent_GuildBossCount != null)
			{
				if (guildEvent_GuildBossCount.ChallengeCount >= 0)
				{
					this.mGuildBossInfo.ChallengeCount = guildEvent_GuildBossCount.ChallengeCount;
				}
				if (guildEvent_GuildBossCount.NextChallengeRecoverTime > 0L)
				{
					this.mGuildBossInfo.NextChallengeRecoverTime = guildEvent_GuildBossCount.NextChallengeRecoverTime;
				}
				if (guildEvent_GuildBossCount.BuyCountDic != null)
				{
					foreach (KeyValuePair<GuildBossBuyKind, GuildBossBuyCountData> keyValuePair in guildEvent_GuildBossCount.BuyCountDic)
					{
						this.mGuildBossInfo.SetBuyCountData(keyValuePair.Value);
					}
				}
			}
		}

		private void OnGuildBossInfoSet(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_SetGuildBossInfo guildEvent_SetGuildBossInfo = eventArgs as GuildEvent_SetGuildBossInfo;
			if (guildEvent_SetGuildBossInfo != null)
			{
				GuildBossInfo guildBossInfo = this.mGuildBossInfo;
				this.mGuildBossInfo = guildEvent_SetGuildBossInfo.Info;
				if (!guildEvent_SetGuildBossInfo.Info.IsFullChallengeRecords)
				{
					this.mGuildBossInfo.CombineRecords(guildBossInfo.ChallengeRecords);
					this.mGuildBossInfo.IsFullChallengeRecords = true;
				}
			}
		}

		private void OnGuildBossTaskSet(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_SetGuildBossTaskData guildEvent_SetGuildBossTaskData = eventArgs as GuildEvent_SetGuildBossTaskData;
			if (guildEvent_SetGuildBossTaskData != null)
			{
				this.UpdateGuildBossTask(guildEvent_SetGuildBossTaskData.TaskList);
			}
		}

		private void OnGuildBossBoxSet(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_SetGuildBossBoxData guildEvent_SetGuildBossBoxData = eventArgs as GuildEvent_SetGuildBossBoxData;
			if (guildEvent_SetGuildBossBoxData != null)
			{
				this.UpdateGuildBossBox(guildEvent_SetGuildBossBoxData.BoxList);
			}
		}

		private void OnGuildBossSetTransID(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_GuildBossSetTransID guildEvent_GuildBossSetTransID = eventArgs as GuildEvent_GuildBossSetTransID;
			if (guildEvent_GuildBossSetTransID != null)
			{
				this.BossBattleTransID = guildEvent_GuildBossSetTransID.BossBattleTransID;
			}
		}

		private void OnCacheGuildUserBossRankList(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_SetGuildUserBossRankList guildEvent_SetGuildUserBossRankList = eventArgs as GuildEvent_SetGuildUserBossRankList;
			if (guildEvent_SetGuildUserBossRankList != null)
			{
				this.mMyGuildRankList.Clear();
				if (guildEvent_SetGuildUserBossRankList.RankList != null)
				{
					this.mMyGuildRankList.AddRange(guildEvent_SetGuildUserBossRankList.RankList);
					this.mMyGuildRankCacheTime = DateTime.Now;
				}
			}
		}

		private void OnCacheGuildBossGuildRankList(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_SetGuildBossGuildRankList guildEvent_SetGuildBossGuildRankList = eventArgs as GuildEvent_SetGuildBossGuildRankList;
			if (guildEvent_SetGuildBossGuildRankList != null)
			{
				this.mGuildRankList.Clear();
				if (guildEvent_SetGuildBossGuildRankList.RankList != null)
				{
					this.mGuildRankList.AddRange(guildEvent_SetGuildBossGuildRankList.RankList);
					this.mGuildRankCacheTime = DateTime.Now;
				}
				if (guildEvent_SetGuildBossGuildRankList.Type == 1)
				{
					this.mMyGuildRankData = guildEvent_SetGuildBossGuildRankList.MyRankData;
					this.mMyGuildRankData.Rank = (int)guildEvent_SetGuildBossGuildRankList.MyRank;
					return;
				}
				if (guildEvent_SetGuildBossGuildRankList.Type == 2)
				{
					this.mMyGuildSelfRankData = guildEvent_SetGuildBossGuildRankList.MyRankData;
					this.mMyGuildSelfRankData.Rank = (int)guildEvent_SetGuildBossGuildRankList.MyRank;
				}
			}
		}

		private void OnGuildBossBoxRewardChange(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_SetGuildBossBoxRewardData guildEvent_SetGuildBossBoxRewardData = eventArgs as GuildEvent_SetGuildBossBoxRewardData;
			if (guildEvent_SetGuildBossBoxRewardData != null)
			{
				this.UpdateGuildBossBoxReward(guildEvent_SetGuildBossBoxRewardData.KillRewardList);
			}
		}

		public GuildBossInfo GuildBoss
		{
			get
			{
				return this.mGuildBossInfo;
			}
		}

		public int GuildBossSkillSeed { get; private set; }

		public GuildBossGuildRankData MyGuildSelfRankData
		{
			get
			{
				return this.mMyGuildSelfRankData;
			}
		}

		public GuildBossGuildRankData MyGuildRankData
		{
			get
			{
				return this.mMyGuildRankData;
			}
		}

		public void SetMyGuildRankEmpty()
		{
			this.mMyGuildRankData = null;
		}

		private void UpdateGuildBossBox(List<GuildBossKillBox> boxlist)
		{
			if (boxlist != null)
			{
				List<GuildBossKillBox> killBoxList = this.mGuildBossInfo.KillBoxList;
				for (int i = 0; i < boxlist.Count; i++)
				{
					GuildBossKillBox guildBossKillBox = boxlist[i];
					bool flag = false;
					for (int j = 0; j < killBoxList.Count; j++)
					{
						if (guildBossKillBox.BoxID == killBoxList[j].BoxID)
						{
							killBoxList[j] = guildBossKillBox;
							flag = true;
						}
					}
					if (!flag)
					{
						killBoxList.Add(guildBossKillBox);
					}
				}
			}
		}

		private void UpdateGuildBossTask(List<GuildBossTask> tasklist)
		{
			if (tasklist != null)
			{
				List<GuildBossTask> taskBossList = this.mGuildBossInfo.TaskBossList;
				for (int i = 0; i < tasklist.Count; i++)
				{
					GuildBossTask guildBossTask = tasklist[i];
					bool flag = false;
					for (int j = 0; j < taskBossList.Count; j++)
					{
						if (guildBossTask.TaskID == taskBossList[j].TaskID)
						{
							taskBossList[j] = guildBossTask;
							flag = true;
						}
					}
					if (!flag)
					{
						taskBossList.Add(guildBossTask);
					}
				}
			}
		}

		private void UpdateGuildBossBoxReward(List<int> killRewardList)
		{
			if (killRewardList != null)
			{
				this.mGuildBossInfo.KillRewardList.Clear();
				this.mGuildBossInfo.KillRewardList.AddRange(killRewardList);
			}
		}

		public bool HasValidMyGuildBossRankList(int timeoutsec = 300)
		{
			if (timeoutsec < 0)
			{
				timeoutsec = 0;
			}
			return this.mMyGuildRankList.Count > 0 && (DateTime.Now - this.mMyGuildRankCacheTime).TotalSeconds <= (double)timeoutsec;
		}

		public List<GuildBossRankData> GetMyGuildBossRankList()
		{
			return this.mMyGuildRankList;
		}

		public GuildBossRankData GetMyGuildBossRankMyData()
		{
			long userID = GuildSDKManager.Instance.User.UserID;
			for (int i = 0; i < this.mMyGuildRankList.Count; i++)
			{
				GuildBossRankData guildBossRankData = this.mMyGuildRankList[i];
				if (guildBossRankData.UserData.UserID == userID)
				{
					return guildBossRankData;
				}
			}
			return null;
		}

		public bool HasValidGuildBossGuildRankList(int timeoutsec = 300)
		{
			if (timeoutsec < 0)
			{
				timeoutsec = 0;
			}
			return this.mGuildRankList.Count > 0 && (DateTime.Now - this.mGuildRankCacheTime).TotalSeconds <= (double)timeoutsec;
		}

		public List<GuildBossGuildRankData> GetGuildBossGuildRankList()
		{
			return this.mGuildRankList;
		}

		public GuildActivityRace GuildRace
		{
			get
			{
				return this.mGuildRace;
			}
		}

		private void OnRaceSetBaseInfo(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_RaceSetBaseInfo guildEvent_RaceSetBaseInfo = eventArgs as GuildEvent_RaceSetBaseInfo;
			if (guildEvent_RaceSetBaseInfo != null)
			{
				this.mGuildRace.InitBaseInfo(guildEvent_RaceSetBaseInfo);
			}
		}

		private void OnRaceApply(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_RaceApply guildEvent_RaceApply = eventArgs as GuildEvent_RaceApply;
			if (guildEvent_RaceApply != null)
			{
				this.mGuildRace.ChangeApplyState(guildEvent_RaceApply);
			}
		}

		private GuildBossInfo mGuildBossInfo = new GuildBossInfo();

		public ulong BossBattleTransID;

		private List<GuildBossRankData> mMyGuildRankList = new List<GuildBossRankData>();

		private DateTime mMyGuildRankCacheTime;

		private List<GuildBossGuildRankData> mGuildRankList = new List<GuildBossGuildRankData>();

		private DateTime mGuildRankCacheTime;

		private GuildBossGuildRankData mMyGuildSelfRankData;

		private GuildBossGuildRankData mMyGuildRankData;

		private GuildActivityRace mGuildRace = new GuildActivityRace();
	}
}
