using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Chapter;
using Server;

namespace HotFix
{
	public class GameEventPoolDataFactory
	{
		public void Init(int randomSeed, int[] difficultArr, int beginSelectSkillNum, int firstEventWithSkill, MapField<uint, EventDetail> serverEventMap, MapField<uint, EventDetail> addActivityMap)
		{
			this.sysRandom = new XRandom(randomSeed);
			this.difficultStage = difficultArr;
			if (Singleton<EventRecordController>.Instance.IsHaveAnyRecord())
			{
				this.UseRecord();
			}
			else
			{
				if (serverEventMap != null)
				{
					foreach (uint num in serverEventMap.Keys)
					{
						EventDetail eventDetail = serverEventMap[num];
						List<GameEventDropData> list = GameEventDropData.ToItemDatas(eventDetail.Drops);
						List<GameEventDropData> list2 = GameEventDropData.ToItemDatas(eventDetail.MonsterDrops);
						List<GameEventDropData> list3 = GameEventDropData.ToItemDatas(eventDetail.BattleDrops);
						GameEventRandomData gameEventRandomData = new GameEventRandomData((int)eventDetail.EventTypeId, (int)num, (int)eventDetail.EventId, eventDetail.Seed, (int)eventDetail.Rate, eventDetail.ActRowId.ToArray<ulong>(), list, list2, list3);
						this.allDataList.Add(gameEventRandomData);
					}
					this.allDataList.Sort((GameEventRandomData a, GameEventRandomData b) => a.stage.CompareTo(b.stage));
					if (beginSelectSkillNum > 0)
					{
						Chapter_eventType elementById = GameApp.Table.GetManager().GetChapter_eventTypeModelInstance().GetElementById(firstEventWithSkill);
						int num2 = this.sysRandom.Range(0, elementById.events.Length);
						int num3 = elementById.events[num2];
						this.allDataList[0].SetPoolId(num3);
						this.allDataList[0].SetId(firstEventWithSkill);
					}
				}
				if (addActivityMap != null)
				{
					foreach (uint num4 in addActivityMap.Keys)
					{
						EventDetail eventDetail2 = addActivityMap[num4];
						List<GameEventDropData> list4 = GameEventDropData.ToItemDatas(eventDetail2.Drops);
						List<GameEventDropData> list5 = GameEventDropData.ToItemDatas(eventDetail2.MonsterDrops);
						List<GameEventDropData> list6 = GameEventDropData.ToItemDatas(eventDetail2.BattleDrops);
						GameEventRandomData gameEventRandomData2 = new GameEventRandomData((int)eventDetail2.EventTypeId, (int)num4, (int)eventDetail2.EventId, eventDetail2.Seed, (int)eventDetail2.Rate, eventDetail2.ActRowId.ToArray<ulong>(), list4, list5, list6);
						gameEventRandomData2.SetAddActivity();
						this.activityEventList.Add(gameEventRandomData2);
					}
				}
				this.activityEventList.Sort((GameEventRandomData a, GameEventRandomData b) => a.stage.CompareTo(b.stage));
				this.randomDataList.AddRange(this.allDataList);
				for (int i = 0; i < this.activityEventList.Count; i++)
				{
					GameEventRandomData gameEventRandomData3 = this.activityEventList[i];
					for (int j = 0; j < this.randomDataList.Count; j++)
					{
						GameEventRandomData gameEventRandomData4 = this.randomDataList[j];
						int num5 = j;
						if (gameEventRandomData4.stage == gameEventRandomData3.stage)
						{
							this.randomDataList.Insert(num5 + 1, gameEventRandomData3);
							break;
						}
					}
				}
				for (int k = 0; k < this.randomDataList.Count; k++)
				{
					this.randomDataList[k].SetIndex(k);
				}
				Singleton<EventRecordController>.Instance.SaveEventQueue(this.allDataList, this.activityEventList);
			}
			this.CreateProgressData();
		}

		private void UseRecord()
		{
			this.randomDataList = Singleton<EventRecordController>.Instance.GetEventQueue();
			this.allDataList.Clear();
			this.allDataList.AddRange(this.randomDataList);
			List<GameEventRandomData> eventActivity = Singleton<EventRecordController>.Instance.GetEventActivity();
			this.activityEventList.Clear();
			this.activityEventList.AddRange(eventActivity);
			for (int i = 0; i < eventActivity.Count; i++)
			{
				eventActivity[i].SetAddActivity();
			}
			for (int j = 0; j < eventActivity.Count; j++)
			{
				GameEventRandomData gameEventRandomData = eventActivity[j];
				for (int k = 0; k < this.randomDataList.Count; k++)
				{
					GameEventRandomData gameEventRandomData2 = this.randomDataList[k];
					int num = k;
					if (gameEventRandomData2.stage == gameEventRandomData.stage)
					{
						this.randomDataList.Insert(num + 1, gameEventRandomData);
						break;
					}
				}
			}
			for (int l = 0; l < this.randomDataList.Count; l++)
			{
				this.randomDataList[l].SetIndex(l);
			}
			int eventRealIndex = Singleton<EventRecordController>.Instance.GetEventRealIndex();
			for (int m = 0; m <= eventRealIndex; m++)
			{
				this.randomDataList.RemoveAt(0);
			}
		}

		public GameEventPoolData GetNext()
		{
			if (this.randomDataList.Count == 0)
			{
				return null;
			}
			GameEventRandomData gameEventRandomData = this.EventQueuePush(true);
			GameEventPoolData eventPoolData;
			if (this.nextPoolData != null)
			{
				eventPoolData = this.nextPoolData;
			}
			else
			{
				eventPoolData = this.GetEventPoolData(gameEventRandomData);
			}
			if (this.randomDataList.Count == 0)
			{
				this.nextPoolData = null;
			}
			else
			{
				GameEventRandomData gameEventRandomData2 = this.EventQueuePush(false);
				this.nextPoolData = this.GetEventPoolData(gameEventRandomData2);
			}
			this.nextShowData = this.nextPoolData;
			return eventPoolData;
		}

		private GameEventRandomData EventQueuePush(bool isRemove)
		{
			ChapterActivityDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterActivityDataModule);
			ChapterBattlePassDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.ChapterBattlePassDataModule);
			GameEventRandomData gameEventRandomData = null;
			for (int i = 0; i < this.randomDataList.Count; i++)
			{
				gameEventRandomData = this.randomDataList[i];
				if (!gameEventRandomData.isAddEvent)
				{
					break;
				}
				bool flag = false;
				for (int j = 0; j < gameEventRandomData.actRowIdArr.Length; j++)
				{
					ulong num = gameEventRandomData.actRowIdArr[j];
					ChapterActivityData activityData = dataModule.GetActivityData(num);
					if (activityData != null && activityData.IsInProgress())
					{
						flag = true;
						break;
					}
					if (dataModule2.BattlePassDto != null && dataModule2.BattlePassDto.RowId == (long)num && dataModule2.IsInProgress())
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			if (gameEventRandomData != null && isRemove)
			{
				int num2 = this.randomDataList.IndexOf(gameEventRandomData);
				for (int k = 0; k <= num2; k++)
				{
					this.randomDataList.RemoveAt(0);
				}
			}
			return gameEventRandomData;
		}

		private GameEventPoolData GetEventPoolData(GameEventRandomData randomData)
		{
			int difficult = this.GetDifficult(randomData.stage);
			GameEventPoolData gameEventPoolData = new GameEventPoolData(randomData.poolId, randomData.stage, randomData.eventType, difficult, randomData.randomSeed, randomData.serverRate, randomData.serverDrops, randomData.monsterDrops, randomData.battleDrops);
			gameEventPoolData.SetData(randomData.queueIndex, randomData.actRowIdArr);
			return gameEventPoolData;
		}

		private void CreateProgressData()
		{
			this.progressList.Clear();
			for (int i = 0; i < this.allDataList.Count; i++)
			{
				GameEventRandomData gameEventRandomData = this.allDataList[i];
				if (gameEventRandomData.stage == 1)
				{
					GameEventProgressData gameEventProgressData = new GameEventProgressData(gameEventRandomData.eventType, gameEventRandomData.stage);
					this.progressList.Add(gameEventProgressData);
				}
				else
				{
					switch (this.allDataList[i].eventType)
					{
					case GameEventType.Select:
					case GameEventType.BattleNormal:
					case GameEventType.BattleElite:
					case GameEventType.BattleBoss:
					case GameEventType.Rest:
					case GameEventType.Fishing:
					{
						GameEventProgressData gameEventProgressData2 = new GameEventProgressData(gameEventRandomData.eventType, gameEventRandomData.stage);
						this.progressList.Add(gameEventProgressData2);
						break;
					}
					}
				}
			}
		}

		public List<GameEventProgressData> GetProgressDataList()
		{
			return this.progressList;
		}

		public EventSizeType GetNextShowSize()
		{
			EventSizeType eventSizeType = EventSizeType.Normal;
			if (this.nextShowData != null)
			{
				eventSizeType = this.nextShowData.eventSizeType;
				this.nextShowData = null;
			}
			else if (this.nextPoolData != null)
			{
				eventSizeType = this.nextPoolData.eventSizeType;
			}
			return eventSizeType;
		}

		public int GetDifficult(int stage)
		{
			int num = 1;
			for (int i = 0; i < this.difficultStage.Length; i++)
			{
				if (stage > this.difficultStage[i])
				{
					num++;
				}
			}
			return num;
		}

		public int GetEventSizeTypeNum(EventSizeType sizeType, int currentStage)
		{
			int num = 0;
			int num2 = 0;
			while (num2 < this.allDataList.Count && this.allDataList[num2].stage <= currentStage)
			{
				Chapter_eventRes chapter_eventRes = GameApp.Table.GetManager().GetChapter_eventRes(this.allDataList[num2].poolId);
				if (chapter_eventRes != null && chapter_eventRes.type == (int)sizeType)
				{
					num++;
				}
				num2++;
			}
			return num;
		}

		private List<GameEventRandomData> randomDataList = new List<GameEventRandomData>();

		private List<GameEventRandomData> allDataList = new List<GameEventRandomData>();

		private List<GameEventRandomData> activityEventList = new List<GameEventRandomData>();

		private List<GameEventProgressData> progressList = new List<GameEventProgressData>();

		private GameEventPoolData nextPoolData;

		private GameEventPoolData nextShowData;

		private XRandom sysRandom;

		private int[] difficultStage;
	}
}
