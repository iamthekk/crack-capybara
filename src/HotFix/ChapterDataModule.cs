using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Battle;
using Proto.Chapter;
using Proto.Common;
using Proto.User;
using Server;
using UnityEngine;

namespace HotFix
{
	public class ChapterDataModule : IDataModule
	{
		public bool isServerBattle { get; private set; }

		public RChapterCombatReq ChapterCombatReq { get; private set; }

		public bool BossBattleResult { get; private set; }

		public int ChapterBattleSeed { get; private set; }

		public BattleUserDto BattleUserDto { get; private set; }

		public string LoginBattleKey { get; private set; }

		public int ChapterBattleTimes { get; private set; }

		public int RandomSeed { get; private set; }

		public MapField<uint, EventDetail> ServerEventMap { get; private set; }

		public MapField<uint, EventDetail> AddActivityMap { get; private set; }

		public string ChapterBattleKey { get; private set; }

		public ChapterData CurrentChapter
		{
			get
			{
				return this.currentChapterData;
			}
		}

		public int ChapterID
		{
			get
			{
				return this.m_chapterID;
			}
		}

		public int MaxStage
		{
			get
			{
				if (this.m_maxStage >= 0)
				{
					return this.m_maxStage;
				}
				return 0;
			}
		}

		public List<ulong> Formation
		{
			get
			{
				return this.m_formation;
			}
		}

		public int ChapterMaxProcess
		{
			get
			{
				return this.ChapterID * this.mChapterIDWaveRate + this.MaxStage + 1;
			}
		}

		public int GetName()
		{
			return 104;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_GameEventData_SetServerData, new HandlerEvent(this.OnEventSetServerData));
			manager.RegisterEvent(LocalMessageName.CC_ChapterData_RefreshChapterRewardData, new HandlerEvent(this.OnEventRefreshChapterRewardData));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_GameEventData_SetServerData, new HandlerEvent(this.OnEventSetServerData));
			manager.UnRegisterEvent(LocalMessageName.CC_ChapterData_RefreshChapterRewardData, new HandlerEvent(this.OnEventRefreshChapterRewardData));
		}

		public void Reset()
		{
			this.m_chapterID = 0;
			this.m_maxStage = 0;
			this.m_formation.Clear();
			this.m_canRewardList = null;
			this.currentChapterData = null;
			this.chapterRewardList.Clear();
			this.ServerEventMap = null;
			this.AddActivityMap = null;
		}

		public void SetLoginData(UserLoginResponse resp)
		{
			this.m_formation.Clear();
			HeroDataModule dataModule = GameApp.Data.GetDataModule(DataName.HeroDataModule);
			this.m_formation = new List<ulong> { (ulong)((long)dataModule.MainCardData.m_rowID) };
			ChapterDto chapter = resp.Chapter;
			if (chapter != null)
			{
				this.m_chapterID = chapter.ChapterId;
				this.m_maxStage = chapter.WaveIndex;
				this.m_canRewardList = chapter.CanRewardList;
				this.LoginBattleKey = chapter.BattleKey;
				this.ChapterBattleTimes = chapter.BattleTimes;
			}
			else
			{
				this.m_chapterID = 1;
				this.m_maxStage = 0;
				this.LoginBattleKey = "0";
				this.ChapterBattleTimes = 0;
			}
			this.currentChapterData = new ChapterData(this.m_chapterID);
			this.InitChapterRewards();
			this.CheckRewardsState();
			GameApp.SDK.Analyze.UserSet(GameTGAUserSetType.Chapter);
		}

		public void SetBossBattleCheckInfo(EndChapterCheckRequest checkRequest, EndChapterCheckResponse checkResponse)
		{
			this.isServerBattle = true;
			this.BossBattleResult = checkResponse.Result >= 1;
			this.ChapterBattleSeed = checkResponse.Seed;
			this.BattleUserDto = checkResponse.UserInfo;
			this.ChapterCombatReq = new RChapterCombatReq
			{
				ChapterId = checkRequest.ChapterId,
				WaveIndex = checkRequest.WaveIndex,
				UserInfo = this.BattleUserDto,
				Seed = checkResponse.Seed,
				CurHp = checkRequest.CurHp,
				ReviveCount = checkRequest.ReviveCount,
				BattleServerLogId = checkResponse.BattleServerLogId,
				BattleServerLogData = checkResponse.BattleServerLogData,
				BattleTimes = this.ChapterBattleTimes
			};
			this.ChapterCombatReq.MonsterCfgId.AddRange(checkRequest.MonsterCfgId);
		}

		public void SetBattleSeed(int battleSeed)
		{
			this.isServerBattle = false;
			this.BossBattleResult = false;
			this.ChapterBattleSeed = battleSeed;
		}

		public void CalcAttributesUpgrade(int stage, GameEventType eventType, out int atkUpgrade, out int hpUpgrade)
		{
			IList<Chapter_stageUpgrade> allElements = GameApp.Table.GetManager().GetChapter_stageUpgradeModelInstance().GetAllElements();
			if (stage == 0)
			{
				atkUpgrade = 0;
				hpUpgrade = 0;
				return;
			}
			if (stage < allElements.Count)
			{
				Chapter_stageUpgrade chapter_stageUpgrade = allElements[stage - 1];
				atkUpgrade = chapter_stageUpgrade.attackUpgrade;
				hpUpgrade = chapter_stageUpgrade.hpUpgrade;
				return;
			}
			Chapter_stageUpgrade chapter_stageUpgrade2 = allElements[allElements.Count - 1];
			int num = stage - allElements.Count;
			atkUpgrade = (int)((float)chapter_stageUpgrade2.attackUpgrade * Mathf.Pow(Singleton<GameConfig>.Instance.GameEvent_Power_AddAttack, (float)num));
			hpUpgrade = (int)((float)chapter_stageUpgrade2.hpUpgrade * Mathf.Pow(Singleton<GameConfig>.Instance.GameEvent_Power_AddHP, (float)num));
		}

		public void RefreshChapterData(int chapterId, int stage, RepeatedField<int> canRewardList, int battleTimes)
		{
			if (this.m_chapterID != chapterId)
			{
				this.currentChapterData = new ChapterData(chapterId);
			}
			this.m_chapterID = chapterId;
			this.m_maxStage = stage;
			this.m_canRewardList = canRewardList;
			this.ChapterBattleTimes = battleTimes;
			this.CheckRewardsState();
			GameApp.SDK.Analyze.UserSet(GameTGAUserSetType.Chapter);
		}

		private void InitChapterRewards()
		{
			this.chapterRewardList.Clear();
			IList<Chapter_chapter> allElements = GameApp.Table.GetManager().GetChapter_chapterModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				Chapter_chapter chapter_chapter = allElements[i];
				if (chapter_chapter.rewardStage.Length != chapter_chapter.dropID.Length)
				{
					HLog.LogError(string.Format("奖励长度不一致，chapterId={0}", chapter_chapter.id));
				}
				for (int j = 0; j < chapter_chapter.rewardStage.Length; j++)
				{
					ChapterRewardData chapterRewardData = new ChapterRewardData();
					chapterRewardData.chapterId = chapter_chapter.id;
					chapterRewardData.stage = chapter_chapter.rewardStage[j];
					chapterRewardData.rewardId = chapter_chapter.dropID[j];
					chapterRewardData.state = ChapterRewardData.ChapterRewardState.Lock;
					this.chapterRewardList.Add(chapterRewardData);
				}
			}
		}

		private void CheckRewardsState()
		{
			for (int i = 0; i < this.chapterRewardList.Count; i++)
			{
				ChapterRewardData chapterRewardData = this.chapterRewardList[i];
				int num = chapterRewardData.chapterId * 1000 + chapterRewardData.stage;
				if (this.m_canRewardList != null && this.m_canRewardList.Contains(num))
				{
					chapterRewardData.state = ChapterRewardData.ChapterRewardState.CanGet;
				}
				else if (chapterRewardData.chapterId < this.ChapterID)
				{
					chapterRewardData.state = ChapterRewardData.ChapterRewardState.Finish;
				}
				else if (chapterRewardData.chapterId > this.ChapterID)
				{
					chapterRewardData.state = ChapterRewardData.ChapterRewardState.Lock;
				}
				else if (chapterRewardData.stage <= this.MaxStage)
				{
					chapterRewardData.state = ChapterRewardData.ChapterRewardState.Finish;
				}
				else
				{
					chapterRewardData.state = ChapterRewardData.ChapterRewardState.Lock;
				}
			}
		}

		private void OnEventSetServerData(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgServerData eventArgServerData = eventArgs as EventArgServerData;
			if (eventArgServerData != null)
			{
				this.RandomSeed = eventArgServerData.Seed;
				this.ServerEventMap = eventArgServerData.EventMap;
				this.AddActivityMap = eventArgServerData.AddActivityMap;
				this.ChapterBattleKey = eventArgServerData.BattleKey;
				this.ChapterBattleTimes = (int)eventArgServerData.BattleTimes;
			}
		}

		private void OnEventRefreshChapterRewardData(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsRefreshChapterRewardData eventArgsRefreshChapterRewardData = eventArgs as EventArgsRefreshChapterRewardData;
			if (eventArgsRefreshChapterRewardData != null)
			{
				this.m_canRewardList = eventArgsRefreshChapterRewardData.m_canRewardList;
				this.CheckRewardsState();
			}
		}

		public List<int> GetPassChapterIds()
		{
			List<int> list = new List<int>();
			IList<Chapter_chapter> allElements = GameApp.Table.GetManager().GetChapter_chapterModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				if (allElements[i].id < this.ChapterID)
				{
					list.Add(allElements[i].id);
				}
			}
			return list;
		}

		public void FinishEvent(int stage, int result, bool isPass)
		{
			int num = PlayerPrefsKeys.GetChapterFightTime(this.ChapterID);
			num++;
			PlayerPrefsKeys.SaveChapterFightTime(this.ChapterID, num);
			this.SendChapterResult(stage, result, isPass);
		}

		public void SendChapterResult(int stage, int result, bool isPass)
		{
			List<BattleChapterDropData> dropDataListExclude = Singleton<GameEventController>.Instance.GetDropDataListExclude(ChapterDropSource.Battle);
			List<BattleChapterDropData> dropDataList = Singleton<GameEventController>.Instance.GetDropDataList(ChapterDropSource.Battle);
			List<RewardDto> list = BattleChapterDropData.ToServerData(dropDataListExclude);
			List<RewardDto> list2 = BattleChapterDropData.ToServerData(dropDataList);
			List<int> battleSkills = Singleton<GameEventController>.Instance.GetBattleSkills();
			NetworkUtils.Chapter.DoEndChapterRequest(this.ChapterID, stage, result, "", this.ChapterBattleKey, list, list2, battleSkills, delegate(bool success, EndChapterResponse resp)
			{
				if (success && resp != null)
				{
					GameEventFinishViewModule.EventEndData eventEndData = new GameEventFinishViewModule.EventEndData();
					eventEndData.chapterId = this.ChapterID;
					eventEndData.endStage = stage;
					eventEndData.result = result;
					eventEndData.isPass = isPass;
					eventEndData.maxStage = ((stage > this.MaxStage) ? stage : this.MaxStage);
					eventEndData.currentTask = this.CheckFinishTaskStage(stage);
					eventEndData.rewardList = resp.CommonData.Reward.ToItemDataList();
					this.RefreshChapterData(resp.ChapterId, resp.WaveIndex, resp.CanRewardList, resp.BattleTimes);
					GameApp.View.OpenView(ViewName.GameEventFinishViewModule, eventEndData, 1, null, null);
				}
			});
		}

		public int GetShowTaskStage()
		{
			for (int i = 0; i < this.CurrentChapter.Config.rewardStage.Length; i++)
			{
				int num = this.CurrentChapter.Config.rewardStage[i];
				if (this.MaxStage < num)
				{
					return num;
				}
			}
			return this.CurrentChapter.Config.totalStage;
		}

		public int CheckFinishTaskStage(int endStage)
		{
			int showTaskStage = this.GetShowTaskStage();
			int num = showTaskStage;
			for (int i = 0; i < this.CurrentChapter.Config.rewardStage.Length; i++)
			{
				int num2 = this.CurrentChapter.Config.rewardStage[i];
				if (num2 > showTaskStage && endStage >= num2)
				{
					num = num2;
				}
			}
			return num;
		}

		public List<ChapterRewardData> GetAllChapterRewardDataList()
		{
			return this.chapterRewardList;
		}

		public ChapterRewardData GetShowReward()
		{
			for (int i = 0; i < this.chapterRewardList.Count; i++)
			{
				if (this.chapterRewardList[i].state == ChapterRewardData.ChapterRewardState.CanGet)
				{
					return this.chapterRewardList[i];
				}
			}
			for (int j = 0; j < this.chapterRewardList.Count; j++)
			{
				if (this.chapterRewardList[j].state == ChapterRewardData.ChapterRewardState.Lock)
				{
					return this.chapterRewardList[j];
				}
			}
			if (this.chapterRewardList.Count > 0)
			{
				return this.chapterRewardList[this.chapterRewardList.Count - 1];
			}
			return null;
		}

		public bool IsPassChapterStage(int chapterId, int stage)
		{
			return chapterId < this.ChapterID || (chapterId == this.ChapterID && stage <= this.MaxStage);
		}

		public int GetPreviousChapterID()
		{
			return this.CurrentChapter.Config.id - 1;
		}

		public static long CalcDynamicDrop(int itemId, long count, float addDropBase, bool isBattle)
		{
			float num = 1f;
			if (isBattle && Singleton<GameEventController>.Instance.ActiveStateName == GameEventStateName.Sweep)
			{
				int num2 = GameApp.Data.GetDataModule(DataName.ChapterSweepDataModule).SweepRate;
				if (num2 < 1)
				{
					num2 = 1;
				}
				num = (float)num2;
			}
			else if (Singleton<GameEventController>.Instance.IsSweepRecord)
			{
				SweepRecordData sweepRecord = Singleton<EventRecordController>.Instance.SweepRecord;
				if (sweepRecord != null && sweepRecord.rate > 1)
				{
					num = (float)sweepRecord.rate;
				}
			}
			if (itemId == 4)
			{
				float num3 = GameApp.Data.GetDataModule(DataName.ChapterDataModule).CurrentChapter.dropBase + addDropBase;
				return ((double)((float)count * num3 * num)).GetValue();
			}
			return ((double)((float)count * num)).GetValue();
		}

		public void UseGlobalRecord()
		{
			EventRecordPlayerData playerRecord = Singleton<EventRecordController>.Instance.PlayerRecord;
			this.RandomSeed = playerRecord.globalSeed;
			this.ChapterBattleKey = playerRecord.battleKey;
		}

		public bool IsShowRateView()
		{
			return this.ChapterID >= 4 && !PlayerPrefsKeys.GetIsRateShow();
		}

		public string GetPVELevelShortName(int id)
		{
			if (this.mChapterIDWaveRate == 0)
			{
				this.mChapterIDWaveRate = 1;
			}
			int num = id / this.mChapterIDWaveRate;
			int num2 = id % this.mChapterIDWaveRate;
			return string.Format("{0}-{1}", num, num2);
		}

		public string GetPVELevelLongName(int id)
		{
			if (this.mChapterIDWaveRate == 0)
			{
				this.mChapterIDWaveRate = 1;
			}
			int num = id / this.mChapterIDWaveRate;
			int num2 = id % this.mChapterIDWaveRate;
			return Singleton<LanguageManager>.Instance.GetInfoByID_LogError(5000, new object[] { num, num2 });
		}

		private int m_chapterID;

		private int m_maxStage;

		private List<ulong> m_formation = new List<ulong>();

		private RepeatedField<int> m_canRewardList;

		private ChapterData currentChapterData;

		private List<ChapterRewardData> chapterRewardList = new List<ChapterRewardData>();

		private int mChapterIDWaveRate = 1000;
	}
}
