using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using LocalModels.Bean;
using Server;

namespace HotFix
{
	public class ChapterData
	{
		public float dropBase { get; private set; } = 1f;

		public int firstEvent { get; private set; }

		public int firstEventWithSkill { get; private set; }

		public int startEventNum { get; private set; }

		public int startEventGood { get; private set; }

		public int startEventBad { get; private set; }

		public int defaultBgm { get; private set; }

		public int battleBgm { get; private set; }

		public int bossBgm { get; private set; }

		public int TotalStage
		{
			get
			{
				return this.Config.totalStage;
			}
		}

		public int UnSelectEventId { get; private set; }

		public int SelectEventId { get; private set; }

		public int CostEnergy { get; private set; }

		public int BigBonus { get; private set; }

		public int BigBonusPlayCount { get; private set; }

		public int MinorBonus { get; private set; }

		public int MinorGameId { get; private set; }

		public Chapter_chapter Config
		{
			get
			{
				if (this._config == null)
				{
					this._config = GameApp.Table.GetManager().GetChapter_chapterModelInstance().GetElementById(this.id);
				}
				return this._config;
			}
		}

		public string Name
		{
			get
			{
				if (this.Config == null)
				{
					return "";
				}
				return Singleton<LanguageManager>.Instance.GetInfoByID(this.Config.nameId);
			}
		}

		public int StageLoop
		{
			get
			{
				if (this.loopEventDataList.Count > 0)
				{
					return this.loopEventDataList[this.loopEventDataList.Count - 1].maxStage;
				}
				return Singleton<GameConfig>.Instance.GameEvent_StageLoop;
			}
		}

		public ChapterData(int id)
		{
			this.id = id;
			this.InitData();
		}

		private void InitData()
		{
			this.queueEventDataList.Clear();
			if (this.Config == null)
			{
				return;
			}
			this.dropBase = this.Config.dropBase;
			if (this.Config.normalEvent.Length >= 2)
			{
				this.UnSelectEventId = this.Config.normalEvent[0];
				this.SelectEventId = this.Config.normalEvent[1];
			}
			if (this.Config.bgm.Length >= 3)
			{
				this.defaultBgm = this.Config.bgm[0];
				this.battleBgm = this.Config.bgm[1];
				this.bossBgm = this.Config.bgm[2];
			}
			if (this.Config.cost.Length >= 2)
			{
				this.CostEnergy = this.Config.cost[1];
			}
			for (int i = 0; i < this.Config.eventQueue.Length; i++)
			{
				ChapterData.ChapterEventData chapterEventData = this.CreateChapterEventData(this.Config.eventQueue[i]);
				this.queueEventDataList.Add(chapterEventData);
			}
			this.loopEventDataList.Clear();
			if (this.Config.startEvent.Length >= 5)
			{
				this.firstEvent = this.Config.startEvent[0];
				this.firstEventWithSkill = this.Config.startEvent[1];
				this.startEventNum = this.Config.startEvent[2];
				this.startEventBad = this.Config.startEvent[3];
				this.startEventGood = this.Config.startEvent[4];
			}
			this.BigBonus = ((this.Config.bigBonus.Length != 0) ? this.Config.bigBonus[0] : 0);
			this.BigBonusPlayCount = ((this.Config.bigBonus.Length > 1) ? this.Config.bigBonus[1] : 0);
			this.MinorBonus = ((this.Config.smallBonus.Length != 0) ? this.Config.smallBonus[0] : 0);
			this.MinorGameId = ((this.Config.smallBonus.Length > 1) ? this.Config.smallBonus[1] : 0);
		}

		private ChapterData.ChapterEventData CreateChapterEventData(string info)
		{
			ChapterData.ChapterEventData chapterEventData = new ChapterData.ChapterEventData();
			string[] array = info.Split(',', StringSplitOptions.None);
			if (array.Length > 2)
			{
				int num;
				chapterEventData.minStage = (int.TryParse(array[0], out num) ? num : 0);
				int num2;
				chapterEventData.maxStage = (int.TryParse(array[1], out num2) ? num2 : 0);
				for (int i = 0; i < array.Length; i++)
				{
					if (i != 0 && i != 1)
					{
						string[] array2 = array[i].Split('&', StringSplitOptions.None);
						int num3;
						int num4;
						if (array2.Length == 2 && int.TryParse(array2[0], out num3) && int.TryParse(array2[1], out num4))
						{
							chapterEventData.AddDic(num3, num4);
						}
						else
						{
							HLog.LogError(string.Format("Chapter eventQueue or eventLoop format error!, id={0} info={1}", this.Config.id, info));
						}
					}
				}
			}
			else
			{
				HLog.LogError(string.Format("Chapter eventQueue or eventLoop format error!, id={0}", this.Config.id));
			}
			return chapterEventData;
		}

		public List<int> GetMonsterGroupList()
		{
			return this.Config.monsterGroup.ToList<int>();
		}

		public int GetMonsterGroup(GameEventBattleType battleType)
		{
			switch (battleType)
			{
			case GameEventBattleType.Normal:
				if (this.Config.monsterGroup.Length == 0)
				{
					return 0;
				}
				return this.Config.monsterGroup[0];
			case GameEventBattleType.Elite:
				if (this.Config.monsterGroup.Length <= 1)
				{
					return 0;
				}
				return this.Config.monsterGroup[1];
			case GameEventBattleType.Boss:
				if (this.Config.monsterGroup.Length <= 2)
				{
					return 0;
				}
				return this.Config.monsterGroup[2];
			default:
				return 0;
			}
		}

		public List<int> GetChapterEventList()
		{
			List<int> list = new List<int>();
			for (int i = 0; i < this.fishingDataList.Count; i++)
			{
				int eventTypeId = this.fishingDataList[i].eventTypeId;
				if (eventTypeId > 0 && !list.Contains(eventTypeId))
				{
					list.Add(eventTypeId);
				}
			}
			for (int j = 0; j < this.Config.normalEvent.Length; j++)
			{
				int num = this.Config.normalEvent[j];
				if (num != 0 && !list.Contains(num))
				{
					list.Add(num);
				}
			}
			for (int k = 0; k < this.queueEventDataList.Count; k++)
			{
				foreach (int num2 in this.queueEventDataList[k].GetDic().Keys)
				{
					if (num2 != 0 && !list.Contains(num2))
					{
						list.Add(num2);
					}
				}
			}
			for (int l = 0; l < this.loopEventDataList.Count; l++)
			{
				foreach (int num3 in this.loopEventDataList[l].GetDic().Keys)
				{
					if (num3 != 0 && !list.Contains(num3))
					{
						list.Add(num3);
					}
				}
			}
			return list;
		}

		public List<ChapterData.ChapterEventData> GetQueueEventDataList()
		{
			return this.queueEventDataList;
		}

		public List<ChapterData.ChapterEventData> GetLoopEventDataList()
		{
			return this.loopEventDataList;
		}

		public List<ChapterData.ChapterFishingData> GetFishingEventDataList()
		{
			return this.fishingDataList;
		}

		public List<MergeAttributeData> GetMergeAttributeDatas()
		{
			return this.Config.attributes.GetMergeAttributeData();
		}

		public List<MergeAttributeData> GetBattleTypeAttributeData(GameEventBattleType battleType)
		{
			switch (battleType)
			{
			case GameEventBattleType.Normal:
				return this.Config.normalBattleAttr.GetMergeAttributeData();
			case GameEventBattleType.Elite:
				return this.Config.eliteBattleAttr.GetMergeAttributeData();
			case GameEventBattleType.Boss:
				return this.Config.bossBattleAttr.GetMergeAttributeData();
			case GameEventBattleType.Npc:
				return this.Config.npcBattleAttr.GetMergeAttributeData();
			default:
				return new List<MergeAttributeData>();
			}
		}

		private List<int[]> SplitIntList(string info)
		{
			List<int[]> list = new List<int[]>();
			string[] array = info.Split('|', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(',', StringSplitOptions.None);
				int[] array3 = new int[array2.Length];
				for (int j = 0; j < array2.Length; j++)
				{
					int.TryParse(array2[j], out array3[j]);
				}
				list.Add(array3);
			}
			return list;
		}

		public int id;

		private List<ChapterData.ChapterEventData> queueEventDataList = new List<ChapterData.ChapterEventData>();

		private List<ChapterData.ChapterEventData> loopEventDataList = new List<ChapterData.ChapterEventData>();

		private List<ChapterData.ChapterFishingData> fishingDataList = new List<ChapterData.ChapterFishingData>();

		private Chapter_chapter _config;

		public class ChapterEventData
		{
			public void AddDic(int typeId, int count)
			{
				if (this.eventDic.ContainsKey(typeId))
				{
					return;
				}
				this.eventDic.Add(typeId, count);
			}

			public Dictionary<int, int> GetDic()
			{
				return this.eventDic;
			}

			public int minStage;

			public int maxStage;

			private Dictionary<int, int> eventDic = new Dictionary<int, int>();
		}

		public class ChapterFishingData
		{
			public int minStartStage;

			public int maxStartStage;

			public int durationStage;

			public int eventTypeId;
		}
	}
}
