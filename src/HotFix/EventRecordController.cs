using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic;
using Proto.Common;
using Server;

namespace HotFix
{
	public class EventRecordController : Singleton<EventRecordController>
	{
		public EventRecordPlayerData PlayerRecord { get; private set; }

		public SweepRecordData SweepRecord { get; private set; }

		public EventRecordEventQueueData EventRecord { get; private set; }

		public List<EventRecordUIData> UIRecordList { get; private set; }

		private ChapterDataModule chapterDataModule
		{
			get
			{
				return GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			}
		}

		private ChapterSweepDataModule sweepDataModule
		{
			get
			{
				return GameApp.Data.GetDataModule(DataName.ChapterSweepDataModule);
			}
		}

		public bool IsHaveAnyRecord()
		{
			return this.IsHaveChapterRecord() || this.IsHaveSweepRecord();
		}

		public bool IsHaveChapterRecord()
		{
			string text = PlayerPrefsKeys.BattleRecordPlayerKey();
			string text2 = PlayerPrefsKeys.BattleRecordEventKey();
			if (Utility.PlayerPrefs.HasKey(text) && Utility.PlayerPrefs.HasKey(text2))
			{
				EventRecordPlayerData playerRecord = this.GetPlayerRecord();
				if (playerRecord != null && playerRecord.chapterId == this.chapterDataModule.CurrentChapter.id && !string.IsNullOrEmpty(playerRecord.battleKey) && !string.IsNullOrEmpty(this.chapterDataModule.LoginBattleKey) && playerRecord.battleKey.Equals(this.chapterDataModule.LoginBattleKey))
				{
					return playerRecord.SaveServerTime <= 0L || playerRecord.SaveServerTime >= DxxTools.Time.ServerTimestamp || DxxTools.Time.ServerTimestamp - playerRecord.SaveServerTime <= 1728000L;
				}
			}
			return false;
		}

		public bool IsHaveSweepRecord()
		{
			string text = PlayerPrefsKeys.SweepRecordKey();
			string text2 = PlayerPrefsKeys.BattleRecordEventKey();
			if (Utility.PlayerPrefs.HasKey(text) && Utility.PlayerPrefs.HasKey(text2))
			{
				SweepRecordData sweepRecord = this.GetSweepRecord();
				if (sweepRecord != null && sweepRecord.chapterId == this.chapterDataModule.CurrentChapter.id - 1)
				{
					return sweepRecord.SaveServerTime <= 0L || sweepRecord.SaveServerTime >= DxxTools.Time.ServerTimestamp || DxxTools.Time.ServerTimestamp - sweepRecord.SaveServerTime <= 1728000L;
				}
			}
			return false;
		}

		public void CreateChapterRecord()
		{
			if (this.IsHaveChapterRecord())
			{
				this.PlayerRecord = this.GetPlayerRecord();
				this.EventRecord = this.GetEventRecord();
				this.UIRecordList = this.GetEventUIRecord(this.EventRecord.eventArr.Length);
				GameTGATools.Ins.OnChapterStart();
				GameTGATools.Ins.SetStageButtonContent(0, Singleton<LanguageManager>.Instance.GetInfoByID(2, "UIGameEvent_NextDay"));
				GameTGATools.Ins.SetStageButtonClickIndex(0);
				GameTGATools.Ins.OnStageClickButton();
				return;
			}
			this.PlayerRecord = new EventRecordPlayerData();
			this.EventRecord = new EventRecordEventQueueData();
			this.UIRecordList = new List<EventRecordUIData>();
		}

		public void CreateSweepRecord()
		{
			if (this.IsHaveSweepRecord())
			{
				this.SweepRecord = this.GetSweepRecord();
				this.EventRecord = this.GetEventRecord();
				return;
			}
			this.SweepRecord = new SweepRecordData();
			this.EventRecord = new EventRecordEventQueueData();
		}

		public int GetEventRealIndex()
		{
			if (Singleton<GameEventController>.Instance.ActiveStateName == GameEventStateName.Chapter)
			{
				return this.PlayerRecord.queueIndex;
			}
			if (Singleton<GameEventController>.Instance.ActiveStateName == GameEventStateName.Sweep)
			{
				return this.SweepRecord.queueIndex;
			}
			return 0;
		}

		public void CacheUI(GameEventUIData data)
		{
			this.cacheUIList.Add(data);
		}

		public void EventGroupEnd()
		{
			if (Singleton<GameEventController>.Instance.ActiveStateName == GameEventStateName.Sweep)
			{
				this.SaveSweepData();
			}
			else
			{
				this.SavePlayerData();
				this.SaveUIGroup();
			}
			this.cacheUIList.Clear();
		}

		public void SaveEventQueue(List<GameEventRandomData> list, List<GameEventRandomData> actList)
		{
			this.EventRecord.EventsToJson(list);
			this.EventRecord.ActivityToJson(actList);
			PlayerPrefsKeys.SaveBattleRecordEvent(JsonManager.SerializeObject(this.EventRecord));
		}

		public void SavePlayerData()
		{
			BattleChapterPlayerData playerData = Singleton<GameEventController>.Instance.PlayerData;
			if (playerData != null)
			{
				this.PlayerRecord.chapterId = this.chapterDataModule.CurrentChapter.id;
				this.PlayerRecord.globalSeed = this.chapterDataModule.RandomSeed;
				this.PlayerRecord.battleKey = this.chapterDataModule.ChapterBattleKey;
				this.PlayerRecord.expLv = playerData.ExpLevel.mVariable;
				this.PlayerRecord.currentExp = playerData.Exp.mVariable;
				this.PlayerRecord.currentHp = (double)playerData.CurrentHp.AsFloat();
				this.PlayerRecord.maxHpPercent = (double)(playerData.AttributeData.HPMaxPercent.AsFloat() * 100f);
				this.PlayerRecord.atkPercent = (double)(playerData.AttributeData.AttackPercent.AsFloat() * 100f);
				this.PlayerRecord.defPercent = (double)(playerData.AttributeData.DefencePercent.AsFloat() * 100f);
				this.PlayerRecord.queueIndex = Singleton<GameEventController>.Instance.GetCurrentEventIndex();
				this.PlayerRecord.playerCoin = playerData.PlayerCoin;
				this.PlayerRecord.playerDiamond = playerData.Diamond;
				this.PlayerRecord.playerChips = playerData.Chips.mVariable;
				this.PlayerRecord.SkillBuildToString(playerData.GetEventSkillBuildIds());
				this.PlayerRecord.EventItemsToJson(Singleton<GameEventController>.Instance.GetAllEventItems());
				this.PlayerRecord.DropDataToJson(Singleton<GameEventController>.Instance.GetDropDataList());
				this.PlayerRecord.BattleEndEffectActiveNum = playerData.BattleEndEffectActiveNum;
				this.PlayerRecord.ReviveCount = playerData.ReviveCount;
				this.PlayerRecord.RefreshSkillCount = playerData.RefreshSkillCount;
				this.PlayerRecord.PlayBigBonusCount = playerData.PlayBigBonusCount;
				this.PlayerRecord.PlayMinorBonusCount = playerData.PlayMinorBonusCount;
				this.PlayerRecord.SaveServerTime = DxxTools.Time.ServerTimestamp;
				PlayerPrefsKeys.SaveBattleRecordPlayer(JsonManager.SerializeObject(this.PlayerRecord));
			}
		}

		private void SaveSweepData()
		{
			this.SweepRecord.chapterId = this.sweepDataModule.SweepChapterId;
			this.SweepRecord.rate = this.sweepDataModule.SweepRate;
			this.SweepRecord.seed = this.sweepDataModule.RandomSeed;
			this.SweepRecord.queueIndex = Singleton<GameEventController>.Instance.GetCurrentEventIndex();
			this.SweepRecord.DropDataToJson(Singleton<GameEventController>.Instance.GetDropDataList());
			this.SweepRecord.SaveServerTime = DxxTools.Time.ServerTimestamp;
			PlayerPrefsKeys.SaveSweepRecord(JsonManager.SerializeObject(this.SweepRecord));
		}

		private void SaveUIGroup()
		{
			for (int i = 0; i < this.cacheUIList.Count; i++)
			{
				if (i == 0)
				{
					PlayerPrefsKeys.SaveBattleRecordUIStage(this.cacheUIList[i].stage, this.cacheUIList.Count);
				}
				EventRecordUIData eventRecordUIData = new EventRecordUIData();
				eventRecordUIData.ToRecordData(this.cacheUIList[i]);
				this.UIRecordList.Add(eventRecordUIData);
				string text = JsonManager.SerializeObject(eventRecordUIData);
				PlayerPrefsKeys.SaveBattleRecordUI(this.cacheUIList[i].stage, i, text);
			}
		}

		public void DeleteChapterRecord()
		{
			Utility.PlayerPrefs.DeleteKey(PlayerPrefsKeys.BattleRecordPlayerKey());
			Utility.PlayerPrefs.DeleteKey(PlayerPrefsKeys.BattleRecordEventKey());
			int totalStage = GameApp.Data.GetDataModule(DataName.ChapterDataModule).CurrentChapter.TotalStage;
			for (int i = 0; i < totalStage; i++)
			{
				int num = i + 1;
				string text = PlayerPrefsKeys.BattleRecordUIStageKey(num);
				if (!Utility.PlayerPrefs.HasKey(text))
				{
					break;
				}
				int @int = Utility.PlayerPrefs.GetInt(text);
				for (int j = 0; j < @int; j++)
				{
					Utility.PlayerPrefs.DeleteKey(PlayerPrefsKeys.BattleRecordUIKey(num, j));
				}
				Utility.PlayerPrefs.DeleteKey(text);
			}
			this.cacheUIList.Clear();
			if (this.UIRecordList != null)
			{
				this.UIRecordList.Clear();
			}
			this.PlayerRecord = null;
			this.EventRecord = null;
		}

		public void DeleteSweepRecord()
		{
			Utility.PlayerPrefs.DeleteKey(PlayerPrefsKeys.SweepRecordKey());
			Utility.PlayerPrefs.DeleteKey(PlayerPrefsKeys.BattleRecordEventKey());
			this.cacheUIList.Clear();
			this.SweepRecord = null;
			this.EventRecord = null;
		}

		private EventRecordPlayerData GetPlayerRecord()
		{
			return JsonManager.ToObject<EventRecordPlayerData>(PlayerPrefsKeys.GetBattleRecordPlayer());
		}

		private EventRecordEventQueueData GetEventRecord()
		{
			return JsonManager.ToObject<EventRecordEventQueueData>(PlayerPrefsKeys.GetBattleRecordEvent());
		}

		private List<EventRecordUIData> GetEventUIRecord(int maxStage)
		{
			List<EventRecordUIData> list = new List<EventRecordUIData>();
			for (int i = 0; i < maxStage; i++)
			{
				int num = i + 1;
				int battleRecordUIStage = PlayerPrefsKeys.GetBattleRecordUIStage(num);
				if (battleRecordUIStage == 0)
				{
					break;
				}
				for (int j = 0; j < battleRecordUIStage; j++)
				{
					EventRecordUIData eventRecordUIData = JsonManager.ToObject<EventRecordUIData>(PlayerPrefsKeys.GetBattleRecordUI(num, j));
					list.Add(eventRecordUIData);
				}
			}
			return list;
		}

		public List<GameEventRandomData> GetEventQueue()
		{
			List<GameEventRandomData> list = new List<GameEventRandomData>();
			for (int i = 0; i < this.EventRecord.eventArr.Length; i++)
			{
				GameEventRandomData gameEventRandomData = JsonManager.ToObject<EventRecordEventData>(this.EventRecord.eventArr[i]).ToEventData();
				list.Add(gameEventRandomData);
			}
			return list;
		}

		public List<GameEventRandomData> GetEventActivity()
		{
			List<GameEventRandomData> list = new List<GameEventRandomData>();
			for (int i = 0; i < this.EventRecord.activityArr.Length; i++)
			{
				GameEventRandomData gameEventRandomData = JsonManager.ToObject<EventRecordEventData>(this.EventRecord.activityArr[i]).ToEventData();
				list.Add(gameEventRandomData);
			}
			return list;
		}

		private SweepRecordData GetSweepRecord()
		{
			return JsonManager.ToObject<SweepRecordData>(PlayerPrefsKeys.GetSweepRecord());
		}

		public int GetRecordStage(GameEventStateName stateName)
		{
			List<GameEventRandomData> eventQueue = this.GetEventQueue();
			List<GameEventRandomData> eventActivity = this.GetEventActivity();
			List<GameEventRandomData> list = new List<GameEventRandomData>();
			list.AddRange(eventQueue);
			list.AddRange(eventActivity);
			list.Sort((GameEventRandomData a, GameEventRandomData b) => a.stage.CompareTo(b.stage));
			if (stateName == GameEventStateName.Sweep)
			{
				if (this.SweepRecord != null && this.EventRecord != null && this.SweepRecord.queueIndex < list.Count)
				{
					return list[this.SweepRecord.queueIndex].stage;
				}
			}
			else if (stateName == GameEventStateName.Chapter && this.PlayerRecord != null && this.EventRecord != null && this.PlayerRecord.queueIndex < list.Count)
			{
				return list[this.PlayerRecord.queueIndex].stage;
			}
			return 0;
		}

		public List<RewardDto> GetEventDropRecord(GameEventStateName stateName)
		{
			List<BattleChapterDropData> list = new List<BattleChapterDropData>();
			if (stateName == GameEventStateName.Sweep)
			{
				if (this.SweepRecord != null && this.EventRecord != null)
				{
					List<BattleChapterDropData> dropList = this.SweepRecord.GetDropList();
					for (int i = 0; i < dropList.Count; i++)
					{
						if (dropList[i].source != ChapterDropSource.Battle)
						{
							list.Add(dropList[i]);
						}
					}
				}
			}
			else if (stateName == GameEventStateName.Chapter && this.PlayerRecord != null && this.EventRecord != null)
			{
				List<BattleChapterDropData> battleChapterDropList = this.PlayerRecord.GetBattleChapterDropList();
				for (int j = 0; j < battleChapterDropList.Count; j++)
				{
					if (battleChapterDropList[j].source != ChapterDropSource.Battle)
					{
						list.Add(battleChapterDropList[j]);
					}
				}
			}
			return BattleChapterDropData.ToServerData(list);
		}

		public List<RewardDto> GetBattleDropRecord(GameEventStateName stateName)
		{
			List<BattleChapterDropData> list = new List<BattleChapterDropData>();
			if (stateName == GameEventStateName.Sweep)
			{
				if (this.SweepRecord != null && this.EventRecord != null)
				{
					List<BattleChapterDropData> dropList = this.SweepRecord.GetDropList();
					for (int i = 0; i < dropList.Count; i++)
					{
						if (dropList[i].source == ChapterDropSource.Battle)
						{
							list.Add(dropList[i]);
						}
					}
				}
			}
			else if (stateName == GameEventStateName.Chapter && this.PlayerRecord != null && this.EventRecord != null)
			{
				List<BattleChapterDropData> battleChapterDropList = this.PlayerRecord.GetBattleChapterDropList();
				for (int j = 0; j < battleChapterDropList.Count; j++)
				{
					if (battleChapterDropList[j].source == ChapterDropSource.Battle)
					{
						list.Add(battleChapterDropList[j]);
					}
				}
			}
			return BattleChapterDropData.ToServerData(list);
		}

		private List<GameEventUIData> cacheUIList = new List<GameEventUIData>();
	}
}
