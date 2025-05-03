using System;
using System.Collections.Generic;
using Server;

namespace HotFix
{
	public abstract class GameEventStateBase : StateMachine.State
	{
		protected GameEventStateBase(int id)
			: base(id)
		{
		}

		public BattleChapterPlayerData GetPlayerData()
		{
			return this.PlayerData;
		}

		public virtual void StartEvent()
		{
		}

		public virtual void ContinueEvent()
		{
		}

		public void PushEvent(GameEventPushType pushType, object param = null)
		{
			if (this.eventFactory == null)
			{
				return;
			}
			this.eventFactory.PushEvent(pushType, param);
		}

		public int GetCurrentEventIndex()
		{
			if (this.eventFactory == null)
			{
				return 0;
			}
			return this.eventFactory.GetCurrentEventIndex();
		}

		public int GetCurrentStage()
		{
			if (this.eventFactory == null)
			{
				return 0;
			}
			return this.eventFactory.GetCurrentStage();
		}

		public int GetDifficult(int stage)
		{
			if (this.poolDataFactory == null)
			{
				return 0;
			}
			return this.poolDataFactory.GetDifficult(stage);
		}

		public int RandomPoolIdByGroup(int group, int difficult, int seed)
		{
			if (this.monsterFactory == null)
			{
				return 0;
			}
			return this.monsterFactory.RandomPoolIdByGroup(group, difficult, seed);
		}

		public int GetMonsterPoolId(int difficult, GameEventType eventType)
		{
			if (this.monsterFactory == null)
			{
				return 0;
			}
			int num;
			if (eventType != GameEventType.BattleElite)
			{
				if (eventType != GameEventType.BattleBoss)
				{
					num = this.monsterFactory.GetBattlePoolId(difficult, GameEventBattleType.Normal);
				}
				else
				{
					num = this.monsterFactory.GetBossPoolId();
				}
			}
			else
			{
				num = this.monsterFactory.GetBattlePoolId(difficult, GameEventBattleType.Elite);
			}
			return num;
		}

		public void HangUp()
		{
			if (this.eventFactory == null)
			{
				return;
			}
			this.eventFactory.HangUp();
		}

		public void ResumeHangUp()
		{
			if (this.eventFactory == null)
			{
				return;
			}
			this.eventFactory.ResumeHangUp();
		}

		public GameEventFishingFactory GetFishingFactory()
		{
			return this.fishingFactory;
		}

		public GameEventPoolDataFactory GetEventPoolDataFactory()
		{
			return this.poolDataFactory;
		}

		public XRandom GetGroupRandom()
		{
			return this.eventFactory.GetGroupRandom();
		}

		public bool IsCurrentEventDone()
		{
			return this.eventFactory.IsCurrentEventDone();
		}

		public void SetReviveCount(int reviveCount)
		{
			if (this.PlayerData == null)
			{
				return;
			}
			this.PlayerData.SetReviveCount(reviveCount);
		}

		public int GetHasReviveCount()
		{
			if (this.PlayerData == null)
			{
				return 0;
			}
			return this.PlayerData.ReviveCount;
		}

		public void UpdateRefreshSkillCount()
		{
			if (this.PlayerData == null)
			{
				return;
			}
			this.PlayerData.UpdateRefreshSkillCount();
		}

		public int GetCanRefreshSkillCount()
		{
			if (this.PlayerData == null)
			{
				return 0;
			}
			return this.PlayerData.GetCanRefreshSkillCount();
		}

		public int GetEventSizeTypeNum(EventSizeType sizeType)
		{
			int currentStage = this.eventFactory.GetCurrentStage();
			return this.poolDataFactory.GetEventSizeTypeNum(sizeType, currentStage);
		}

		public int GetRandomBox(int seed)
		{
			if (this.boxBuildBuildPool == null)
			{
				return 0;
			}
			return this.boxBuildBuildPool.RandomBox(seed);
		}

		public int GetCurrentBoxId()
		{
			if (this.boxBuildBuildPool == null)
			{
				return 0;
			}
			return this.boxBuildBuildPool.GetCurrentBoxId();
		}

		public void SetFixBoxId(int boxId, int seed)
		{
			if (this.boxBuildBuildPool == null)
			{
				return;
			}
			this.boxBuildBuildPool.SetFixBoxId(boxId, seed);
		}

		public int GetCurBoxSkillNum()
		{
			if (this.boxBuildBuildPool == null)
			{
				return 0;
			}
			return this.boxBuildBuildPool.GetCurBoxSkillNum();
		}

		public void AddDrop(BattleChapterDropData data)
		{
			if (data == null)
			{
				return;
			}
			if (this.dropDataCtrl != null)
			{
				this.dropDataCtrl.AddDrop(data);
			}
		}

		public void AddDrops(List<NodeItemParam> list)
		{
			if (this.PlayerData != null)
			{
				this.PlayerData.AddDrops(list);
			}
			if (this.dropDataCtrl != null)
			{
				this.dropDataCtrl.AddDropList(list);
			}
		}

		public List<BattleChapterDropData> GetDropDataList()
		{
			if (this.dropDataCtrl == null)
			{
				return new List<BattleChapterDropData>();
			}
			return this.dropDataCtrl.GetDropDataList();
		}

		public List<BattleChapterDropData> GetDropDataList(ChapterDropSource dropSource)
		{
			if (this.dropDataCtrl == null)
			{
				return new List<BattleChapterDropData>();
			}
			return this.dropDataCtrl.GetDropDataList(dropSource);
		}

		public List<BattleChapterDropData> GetDropDataListExclude(ChapterDropSource dropSource)
		{
			if (this.dropDataCtrl == null)
			{
				return new List<BattleChapterDropData>();
			}
			return this.dropDataCtrl.GetDropDataListExclude(dropSource);
		}

		public List<PropData> GetDropDataShowList()
		{
			if (this.dropDataCtrl == null)
			{
				return new List<PropData>();
			}
			return this.dropDataCtrl.GetDropDataShowList();
		}

		public void AddEventItem(int id, int num, int stage)
		{
			if (this.itemFactory == null)
			{
				return;
			}
			this.itemFactory.AddEventItem(id, num, stage);
		}

		public List<GameEventItemData> GetItemsByType(EventItemType type)
		{
			if (this.itemFactory == null)
			{
				return new List<GameEventItemData>();
			}
			return this.itemFactory.GetItemsByType(EventItemType.FishRod);
		}

		public List<GameEventItemData> GetAllEventItems()
		{
			if (this.itemFactory == null)
			{
				return new List<GameEventItemData>();
			}
			return this.itemFactory.GetItems();
		}

		public List<GameEventItemData> GetShowItems()
		{
			if (this.itemFactory == null)
			{
				return new List<GameEventItemData>();
			}
			return this.itemFactory.GetShowItemDatas();
		}

		public bool IsHaveEventItem(int itemId)
		{
			return this.itemFactory != null && this.itemFactory.IsHaveEventItem(itemId);
		}

		public bool IsItemsActiveEvent(int[] items)
		{
			return this.itemFactory != null && this.itemFactory.IsItemsActiveEvent(items);
		}

		public void CheckEventItemRemove(int currentStage)
		{
			if (this.itemFactory == null)
			{
				return;
			}
			this.itemFactory.CheckEventItemRemove(currentStage);
		}

		public bool IsEventItemBuyEnabled(int itemId, int num)
		{
			return this.itemFactory != null && this.itemFactory.IsEventItemBuyEnabled(itemId, num);
		}

		public void EventItemBuy(int itemId, int num)
		{
			if (this.itemFactory == null)
			{
				return;
			}
			this.itemFactory.EventItemBuy(itemId, num);
		}

		public int GetLostFood()
		{
			if (this.itemFactory == null)
			{
				return 0;
			}
			return this.itemFactory.GetLostFood();
		}

		public void AddRecordItems(List<GameEventItemData> record)
		{
			if (this.itemFactory == null)
			{
				return;
			}
			this.itemFactory.AddRecordItems(record);
		}

		public List<GameEventSkillBuildData> GetRandomSkillList(SkillBuildSourceType sourceType, int randomNum, int seed)
		{
			if (this.skillBuildPool == null)
			{
				return new List<GameEventSkillBuildData>();
			}
			return this.skillBuildPool.GetRandomList(sourceType, randomNum, seed);
		}

		public List<int> AddSkillAndCheckRemove(GameEventSkillBuildData data)
		{
			List<int> list = new List<int>();
			if (this.PlayerData == null)
			{
				return list;
			}
			if (data.level > 1)
			{
				GameEventSkillBuildData skillBuildByGroup = Singleton<GameEventController>.Instance.GetSkillBuildByGroup(data.groupId);
				if (skillBuildByGroup != null)
				{
					this.PlayerData.ReplaceSkillBuild(skillBuildByGroup, data);
					if (skillBuildByGroup.skillId > 0)
					{
						list.Add(skillBuildByGroup.skillId);
					}
				}
			}
			else
			{
				this.PlayerData.AddSkillBuild(data, true);
			}
			if (data.IsComposeSkill && data.composeType == SkillBuildComposeType.Replace)
			{
				for (int i = 0; i < data.composeArr.Length; i++)
				{
					GameEventSkillBuildData skillBuild = Singleton<GameEventController>.Instance.GetSkillBuild(data.composeArr[i]);
					if (skillBuild != null)
					{
						List<int> list2 = this.PlayerData.RemoveSkillBuildByGroup(skillBuild.groupId);
						list.AddRange(list2);
					}
				}
			}
			return list;
		}

		public void SelectSkill(GameEventSkillBuildData data, bool checkUnlock)
		{
			if (this.skillBuildPool == null)
			{
				return;
			}
			this.skillBuildPool.SelectSkill(data, checkUnlock);
		}

		public GameEventSkillBuildData GetSkillByID(int skillBuildId)
		{
			if (this.skillBuildPool == null)
			{
				return null;
			}
			return this.skillBuildPool.GetSkillByID(skillBuildId);
		}

		public int GetSkillBuildGroupMaxLevel(int groupId)
		{
			if (this.skillBuildPool == null)
			{
				return 0;
			}
			return this.skillBuildPool.GetSkillBuildGroupMaxLevel(groupId);
		}

		public GameEventSkillBuildData GetSpecifiedSkill(int buildId)
		{
			if (this.skillBuildPool == null)
			{
				return null;
			}
			return this.skillBuildPool.GetSpecifiedSkill(buildId);
		}

		public bool IsSkillPoolEmpty(SkillBuildSourceType sourceType)
		{
			return this.skillBuildPool == null || this.skillBuildPool.IsSkillPoolEmpty(sourceType);
		}

		public GameEventSkillBuildData RandomLostSkill(int tag, int seed)
		{
			if (this.skillBuildPool == null)
			{
				return null;
			}
			return this.skillBuildPool.RandomLostSkill(tag, seed);
		}

		public void LostSkill(GameEventSkillBuildData skillBuild)
		{
			if (this.skillBuildPool == null)
			{
				return;
			}
			this.skillBuildPool.LostSkill(skillBuild);
		}

		public List<GameEventSkillBuildData> GetUnlockSkills()
		{
			if (this.skillBuildPool == null)
			{
				return new List<GameEventSkillBuildData>();
			}
			return this.skillBuildPool.GetUnlockSkills();
		}

		public void RemoveUnlockSkill(int index)
		{
			if (this.skillBuildPool == null)
			{
				return;
			}
			this.skillBuildPool.RemoveUnlockSkill(index);
		}

		public List<GameEventSkillBuildData> GetInitSkillBuildList()
		{
			if (this.skillBuildPool == null)
			{
				return new List<GameEventSkillBuildData>();
			}
			return this.skillBuildPool.GetInitSkillBuildList();
		}

		public int GetLevelUpSkillSeed(int lv)
		{
			if (this.skillBuildPool == null)
			{
				return 0;
			}
			return this.skillBuildPool.GetLevelUpSkillSeed(lv);
		}

		public int GetRefreshSkillSeed(int refreshCount, bool isAd)
		{
			if (this.skillBuildPool == null)
			{
				return 0;
			}
			return this.skillBuildPool.GetRefreshSkillSeed(refreshCount, isAd);
		}

		public List<GameEventSkillBuildData> GetSkillPool(SkillBuildSourceType sourceType)
		{
			if (this.skillBuildPool == null)
			{
				return new List<GameEventSkillBuildData>();
			}
			return this.skillBuildPool.GetSkillPool(sourceType);
		}

		public int GetRandomSurprise(int buildId, int seed)
		{
			if (this.boxBuildBuildPool == null)
			{
				return 0;
			}
			return this.surpriseBuildPool.RandomSurprise(buildId, seed);
		}

		public int GetCurrentSurpriseId()
		{
			if (this.boxBuildBuildPool == null)
			{
				return 0;
			}
			return this.surpriseBuildPool.GetCurrentSurpriseId();
		}

		public void SetCurrentSurpriseId(int surpriseId)
		{
			if (this.boxBuildBuildPool == null)
			{
				return;
			}
			this.surpriseBuildPool.SetCurrentSurpriseId(surpriseId);
		}

		public GameEventAttributesFactory.AttributeBuild RandomAttributeBuildId(int seed)
		{
			if (this.attributeBuildPool == null)
			{
				return null;
			}
			return this.attributeBuildPool.RandomAttribute(seed);
		}

		public List<GameEventSlotTrainFactory.SlotTrainBuild> CreateSlotTrainBuilds(List<GameEventSkillBuildData> skills, int seed)
		{
			if (this.slotTrainPool == null)
			{
				return new List<GameEventSlotTrainFactory.SlotTrainBuild>();
			}
			return this.slotTrainPool.CreateBuilds(skills, seed);
		}

		public GameEventSlotTrainFactory.SlotTrainBuild RandomSlotTrain()
		{
			if (this.slotTrainPool == null)
			{
				return null;
			}
			return this.slotTrainPool.RandomSlotTrain();
		}

		protected BattleChapterDropDataCtrl dropDataCtrl;

		protected GameEventPoolDataFactory poolDataFactory;

		protected GameEventMonsterFactory monsterFactory;

		protected GameEventFactory eventFactory;

		protected GameEventBoxBuildFactory boxBuildBuildPool;

		protected GameEventSurpriseBuildFactory surpriseBuildPool;

		protected GameEventAttributesFactory attributeBuildPool;

		protected GameEventSlotTrainFactory slotTrainPool;

		protected GameEventSkillBuildFactory skillBuildPool;

		protected GameEventItemFactory itemFactory;

		protected GameEventFishingFactory fishingFactory;

		protected BattleChapterPlayerData PlayerData;
	}
}
