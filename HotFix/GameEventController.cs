using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using Server;

namespace HotFix
{
	public class GameEventController : Singleton<GameEventController>
	{
		private StateMachine StateMachine
		{
			get
			{
				if (this._stateMachine == null)
				{
					this.RegisterState();
				}
				return this._stateMachine;
			}
		}

		public GameEventStateBase CurrentState
		{
			get
			{
				int currentStateName = this.StateMachine.GetCurrentStateName();
				return this.StateMachine.GetState<GameEventStateBase>(currentStateName);
			}
		}

		public BattleChapterPlayerData PlayerData
		{
			get
			{
				return this.CurrentState.GetPlayerData();
			}
		}

		public GameEventStateName ActiveStateName { get; private set; } = GameEventStateName.Idle;

		public bool IsSweepRecord { get; private set; }

		private void RegisterState()
		{
			if (this.isInit)
			{
				return;
			}
			this.isInit = true;
			GameEventStateIdle gameEventStateIdle = new GameEventStateIdle(-1);
			GameEventStateChapter gameEventStateChapter = new GameEventStateChapter(0);
			GameEventStateSweep gameEventStateSweep = new GameEventStateSweep(1);
			GameEventStateRogueDungeon gameEventStateRogueDungeon = new GameEventStateRogueDungeon(2);
			GameEventStateWorldBoss gameEventStateWorldBoss = new GameEventStateWorldBoss(3);
			GameEventStateGuildBoss gameEventStateGuildBoss = new GameEventStateGuildBoss(4);
			this._stateMachine = new StateMachine();
			this._stateMachine.RegisterState(gameEventStateIdle);
			this._stateMachine.RegisterState(gameEventStateChapter);
			this._stateMachine.RegisterState(gameEventStateSweep);
			this._stateMachine.RegisterState(gameEventStateRogueDungeon);
			this._stateMachine.RegisterState(gameEventStateWorldBoss);
			this._stateMachine.RegisterState(gameEventStateGuildBoss);
		}

		public bool IsState(GameEventStateName name)
		{
			return name == this.ActiveStateName;
		}

		public void EnterEventMode(GameEventStateName state)
		{
			this.ActiveStateName = state;
			this.lastLearnSkills.Clear();
			this.StateMachine.ActiveState((int)state);
		}

		public void SetTempSweep(bool isTemp)
		{
			this.IsSweepRecord = isTemp;
		}

		public void StartEvent()
		{
			this.CurrentState.StartEvent();
		}

		public void ContinueEvent()
		{
			this.CurrentState.ContinueEvent();
		}

		public void PushEvent(GameEventPushType pushType, object param = null)
		{
			this.CurrentState.PushEvent(pushType, param);
		}

		public int GetCurrentEventIndex()
		{
			return this.CurrentState.GetCurrentEventIndex();
		}

		public int GetCurrentStage()
		{
			return this.CurrentState.GetCurrentStage();
		}

		public int GetTotalStage()
		{
			if (this.CurrentState.ID == 0)
			{
				return GameApp.Data.GetDataModule(DataName.ChapterDataModule).CurrentChapter.TotalStage;
			}
			if (this.CurrentState.ID == 1)
			{
				ChapterSweepDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterSweepDataModule);
				Chapter_chapter elementById = GameApp.Table.GetManager().GetChapter_chapterModelInstance().GetElementById(dataModule.SweepChapterId);
				if (elementById != null)
				{
					return elementById.journeyStage;
				}
			}
			return 0;
		}

		public int GetDifficult(int stage)
		{
			return this.CurrentState.GetDifficult(stage);
		}

		public void HangUp()
		{
			this.CurrentState.HangUp();
		}

		public void ResumeHangUp()
		{
			this.CurrentState.ResumeHangUp();
		}

		public bool IsCurrentEventDone()
		{
			return this.CurrentState.IsCurrentEventDone();
		}

		public void SetReviveCount(int reviveCount)
		{
			this.CurrentState.SetReviveCount(reviveCount);
		}

		public int GetHasReviveCount()
		{
			return this.CurrentState.GetHasReviveCount();
		}

		public void UpdateRefreshSkillCount()
		{
			this.CurrentState.UpdateRefreshSkillCount();
		}

		public int GetCanRefreshSkillCount()
		{
			return this.CurrentState.GetCanRefreshSkillCount();
		}

		public void MergerAttribute(NodeAttParam attParam)
		{
			List<NodeAttParam> list = new List<NodeAttParam> { attParam };
			this.MergerAttribute(list);
		}

		public void MergerAttribute(List<NodeAttParam> attParams)
		{
			if (this.PlayerData != null)
			{
				Dictionary<GameEventAttType, double> dictionary = new Dictionary<GameEventAttType, double>();
				for (int i = 0; i < attParams.Count; i++)
				{
					NodeAttParam nodeAttParam = attParams[i];
					if (dictionary.ContainsKey(nodeAttParam.attType))
					{
						Dictionary<GameEventAttType, double> dictionary2 = dictionary;
						GameEventAttType attType = nodeAttParam.attType;
						dictionary2[attType] += nodeAttParam.FinalCount;
					}
					else
					{
						dictionary.Add(nodeAttParam.attType, nodeAttParam.FinalCount);
					}
				}
				foreach (GameEventAttType gameEventAttType in dictionary.Keys)
				{
					GameEventAttType gameEventAttType2 = gameEventAttType;
					double num = dictionary[gameEventAttType];
					if (gameEventAttType2 == GameEventAttType.Exp)
					{
						this.PlayerData.UpdateAttribute(gameEventAttType2, num);
						GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_RefreshExp, null);
					}
					else
					{
						this.PlayerData.UpdateAttribute(gameEventAttType2, num);
					}
				}
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_RefreshAttribute, null);
			}
		}

		public string GetCheckAttribute()
		{
			long value = (this.PlayerData.AttributeData.AttackPercent.AsFloat() * 100f).GetValue();
			long value2 = (this.PlayerData.AttributeData.DefencePercent.AsFloat() * 100f).GetValue();
			long value3 = (this.PlayerData.AttributeData.HPMaxPercent.AsFloat() * 100f).GetValue();
			string text = "Attack%=" + value.ToString();
			string text2 = "Defence%=" + value2.ToString();
			string text3 = "HPMax%=" + value3.ToString();
			return string.Concat(new string[] { text, "|", text2, "|", text3 });
		}

		public void LevelUpRecoverHp(int lv)
		{
			EventArgLevelupHp eventArgLevelupHp = new EventArgLevelupHp();
			eventArgLevelupHp.SetLevel(lv);
			if (this.PlayerData != null)
			{
				float num = this.PlayerData.AttributeData.UpgradeRegeneRate.AsFloat() * 100f;
				if (num > 0f)
				{
					NodeAttParam nodeAttParam = new NodeAttParam(GameEventAttType.RecoverHpRate, (double)num, ChapterDropSource.Event, 1);
					List<NodeAttParam> list = new List<NodeAttParam> { nodeAttParam };
					this.MergerAttribute(list);
					eventArgLevelupHp.SetAttData(list);
				}
			}
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_ShowLevelUp, eventArgLevelupHp);
		}

		public int GetMonsterPoolId(int difficult, GameEventType eventType)
		{
			return this.CurrentState.GetMonsterPoolId(difficult, eventType);
		}

		public int RandomPoolIdByGroup(int group, int difficult, int seed)
		{
			return this.CurrentState.RandomPoolIdByGroup(group, difficult, seed);
		}

		public GameEventFishingFactory GetFishingFactory()
		{
			return this.CurrentState.GetFishingFactory();
		}

		public GameEventPoolDataFactory GetEventPoolDataFactory()
		{
			return this.CurrentState.GetEventPoolDataFactory();
		}

		public List<BattleChapterDropData> GetDropDataList()
		{
			return this.CurrentState.GetDropDataList();
		}

		public List<BattleChapterDropData> GetDropDataList(ChapterDropSource dropSource)
		{
			return this.CurrentState.GetDropDataList(dropSource);
		}

		public List<BattleChapterDropData> GetDropDataListExclude(ChapterDropSource dropSource)
		{
			return this.CurrentState.GetDropDataListExclude(dropSource);
		}

		public void AddDrop(NodeItemParam item)
		{
			List<NodeItemParam> list = new List<NodeItemParam> { item };
			this.AddDrops(list);
		}

		public void AddDrops(List<NodeItemParam> list)
		{
			this.CurrentState.AddDrops(list);
		}

		public void AddDrop(BattleChapterDropData data)
		{
			if (data == null)
			{
				return;
			}
			this.CurrentState.AddDrop(data);
		}

		public long GetDropItemCount(int id)
		{
			long num = 0L;
			List<BattleChapterDropData> dropDataList = this.CurrentState.GetDropDataList();
			for (int i = 0; i < dropDataList.Count; i++)
			{
				if (dropDataList[i].id == id || (id == 1 && dropDataList[i].id == 4))
				{
					num += dropDataList[i].finalCount;
				}
			}
			return num;
		}

		public float GetAddDropBase(int id, ChapterDropSource source)
		{
			float num = 0f;
			MemberAttributeData memberAttributeData;
			if (this.PlayerData != null && this.PlayerData.AttributeData != null)
			{
				memberAttributeData = this.PlayerData.AttributeData;
			}
			else
			{
				memberAttributeData = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule).MemberAttributeData;
			}
			if (id == 4)
			{
				switch (source)
				{
				case ChapterDropSource.Event:
					num = memberAttributeData.EventCoinAddRate.AsFloat();
					break;
				case ChapterDropSource.Battle:
					num = memberAttributeData.BattleCoinAddRate.AsFloat();
					break;
				case ChapterDropSource.TinySlot:
					num = memberAttributeData.SlotCoinAddRate.AsFloat();
					break;
				case ChapterDropSource.CardFlipping:
					num = memberAttributeData.FlippingCoinAddRate.AsFloat();
					break;
				}
			}
			return num;
		}

		public bool IsEventItemBuyEnabled(int itemId, int num)
		{
			return this.CurrentState.IsEventItemBuyEnabled(itemId, num);
		}

		public void EventItemBuy(int itemId, int num)
		{
			this.CurrentState.EventItemBuy(itemId, num);
		}

		public bool IsHaveEventItem(int itemId)
		{
			return this.CurrentState.IsHaveEventItem(itemId);
		}

		public List<GameEventItemData> GetAllEventItems()
		{
			return this.CurrentState.GetAllEventItems();
		}

		public void AddEventItem(int id, int num, int stage)
		{
			this.CurrentState.AddEventItem(id, num, stage);
		}

		public void CheckEventItemRemove(int currentStage)
		{
			this.CurrentState.CheckEventItemRemove(currentStage);
		}

		public bool IsItemsActiveEvent(int[] items)
		{
			return this.CurrentState.IsItemsActiveEvent(items);
		}

		public List<GameEventItemData> GetShowItems()
		{
			return this.CurrentState.GetShowItems();
		}

		public void AddRecordItems(List<GameEventItemData> record)
		{
			this.CurrentState.AddRecordItems(record);
		}

		public bool IsBuyEnabled(int num)
		{
			return this.PlayerData == null || this.PlayerData.Food >= num;
		}

		public void Buy(int num)
		{
			if (this.IsBuyEnabled(num))
			{
				NodeAttParam nodeAttParam = new NodeAttParam(GameEventAttType.Food, (double)(-(double)num), ChapterDropSource.Event, 1);
				GameTGATools.Ins.AddStageClickTempAtt(new List<NodeAttParam> { nodeAttParam }, true);
				this.MergerAttribute(new List<NodeAttParam> { nodeAttParam });
			}
		}

		public bool BuySlotTrain(int playCount)
		{
			int slotTrainPrice = GameConfig.GetSlotTrainPrice(playCount);
			if (slotTrainPrice < 0)
			{
				return false;
			}
			if (slotTrainPrice == 0)
			{
				return true;
			}
			NodeAttParam nodeAttParam = new NodeAttParam(GameEventAttType.RecoverHpRate, (double)(-(double)slotTrainPrice), ChapterDropSource.Event, 1);
			GameTGATools.Ins.AddStageClickTempAtt(new List<NodeAttParam> { nodeAttParam }, true);
			this.MergerAttribute(new List<NodeAttParam> { nodeAttParam });
			return true;
		}

		public bool BuyRefreshSlotTrain()
		{
			if (this.PlayerData.Food >= GameConfig.GameEvent_SlotTrain_Refresh_Price)
			{
				int num = -GameConfig.GameEvent_SlotTrain_Refresh_Price;
				NodeAttParam nodeAttParam = new NodeAttParam(GameEventAttType.Food, (double)num, ChapterDropSource.Event, 1);
				GameTGATools.Ins.AddStageClickTempAtt(new List<NodeAttParam> { nodeAttParam }, true);
				this.MergerAttribute(new List<NodeAttParam> { nodeAttParam });
				return true;
			}
			EventArgsString instance = Singleton<EventArgsString>.Instance;
			instance.SetData(Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_113"));
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_TipViewModule_AddItemTipNode, instance);
			return false;
		}

		public List<GameEventItemData> GetItemsByType(EventItemType type)
		{
			return this.CurrentState.GetItemsByType(EventItemType.FishRod);
		}

		public void LostSkill(GameEventSkillBuildData skillBuild)
		{
			this.CurrentState.LostSkill(skillBuild);
			if (this.PlayerData != null)
			{
				this.PlayerData.RemoveSkillBuild(skillBuild.id);
			}
		}

		public List<GameEventSkillBuildData> GetUnlockSkills()
		{
			return this.CurrentState.GetUnlockSkills();
		}

		public void RemoveUnlockSkill(int index)
		{
			this.CurrentState.RemoveUnlockSkill(index);
		}

		public GameEventSkillBuildData GetSkillByID(int skillBuildId)
		{
			return this.CurrentState.GetSkillByID(skillBuildId);
		}

		public void AddSkill(GameEventSkillBuildData data, bool checkUnlock)
		{
			this.CurrentState.SelectSkill(data, checkUnlock);
		}

		public bool IsHaveSkill(int skillId)
		{
			if (this.PlayerData != null)
			{
				List<GameEventSkillBuildData> playerSkillBuildList = this.PlayerData.GetPlayerSkillBuildList();
				for (int i = 0; i < playerSkillBuildList.Count; i++)
				{
					if (playerSkillBuildList[i].skillId == skillId)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool IsHaveSkillBuild(int buildId)
		{
			if (this.PlayerData != null)
			{
				List<GameEventSkillBuildData> playerSkillBuildList = this.PlayerData.GetPlayerSkillBuildList();
				for (int i = 0; i < playerSkillBuildList.Count; i++)
				{
					if (playerSkillBuildList[i].id == buildId)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool IsHaveSkillGroupId(int groupId)
		{
			if (this.PlayerData != null)
			{
				List<GameEventSkillBuildData> playerSkillBuildList = this.PlayerData.GetPlayerSkillBuildList();
				for (int i = 0; i < playerSkillBuildList.Count; i++)
				{
					if (playerSkillBuildList[i].groupId == groupId)
					{
						return true;
					}
				}
			}
			return false;
		}

		public GameEventSkillBuildData GetSkillBuild(int buildId)
		{
			if (this.PlayerData != null)
			{
				List<GameEventSkillBuildData> playerSkillBuildList = this.PlayerData.GetPlayerSkillBuildList();
				for (int i = 0; i < playerSkillBuildList.Count; i++)
				{
					if (playerSkillBuildList[i].id == buildId)
					{
						return playerSkillBuildList[i];
					}
				}
			}
			return null;
		}

		public GameEventSkillBuildData GetSkillBuildByGroup(int groupId)
		{
			if (this.PlayerData != null)
			{
				List<GameEventSkillBuildData> playerSkillBuildList = this.PlayerData.GetPlayerSkillBuildList();
				for (int i = 0; i < playerSkillBuildList.Count; i++)
				{
					if (playerSkillBuildList[i].groupId == groupId)
					{
						return playerSkillBuildList[i];
					}
				}
			}
			return null;
		}

		public void SelectSkill(GameEventSkillBuildData data, bool isCacheLearn)
		{
			if (this.GetSkillBuild(data.id) != null && data.id != 100000)
			{
				return;
			}
			if (isCacheLearn)
			{
				this.lastLearnSkills.Add(data);
			}
			List<int> list = this.CurrentState.AddSkillAndCheckRemove(data);
			this.CurrentState.SelectSkill(data, true);
			EventArgsEventSelectSkill eventArgsEventSelectSkill = new EventArgsEventSelectSkill();
			eventArgsEventSelectSkill.SetData(new List<int> { data.skillId }, list, data);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Battle_EventSelectSkill, eventArgsEventSelectSkill);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_RefreshAttribute, null);
		}

		public List<GameEventSkillBuildData> GetLastLearnSkill()
		{
			return this.lastLearnSkills;
		}

		public void ClearLastLearnSkill()
		{
			this.lastLearnSkills.Clear();
		}

		public List<GameEventSkillBuildData> GetLostSkills(int tag)
		{
			List<GameEventSkillBuildData> list = new List<GameEventSkillBuildData>();
			List<GameEventSkillBuildData> playerSkillBuildList = this.PlayerData.GetPlayerSkillBuildList();
			for (int i = 0; i < playerSkillBuildList.Count; i++)
			{
				if (playerSkillBuildList[i].tag == tag)
				{
					list.Add(playerSkillBuildList[i]);
				}
			}
			return list;
		}

		public List<int> GetBattleSkills()
		{
			return this.PlayerData.GetEventSkillIDs();
		}

		public bool IsSkillPoolEmpty(SkillBuildSourceType sourceType)
		{
			return this.CurrentState.IsSkillPoolEmpty(sourceType);
		}

		public GameEventSkillBuildData RandomLostSkill(int tag, int seed)
		{
			return this.CurrentState.RandomLostSkill(tag, seed);
		}

		public List<GameEventSkillBuildData> GetRandomSkillList(SkillBuildSourceType sourceType, int randomNum, int seed)
		{
			return this.CurrentState.GetRandomSkillList(sourceType, randomNum, seed);
		}

		public int GetSkillBuildGroupMaxLevel(int groupId)
		{
			return this.CurrentState.GetSkillBuildGroupMaxLevel(groupId);
		}

		public GameEventSkillBuildData GetSpecifiedSkill(int buildId)
		{
			return this.CurrentState.GetSpecifiedSkill(buildId);
		}

		public int GetLevelUpSkillSeed(int lv)
		{
			return this.CurrentState.GetLevelUpSkillSeed(lv);
		}

		public int GetRefreshSkillSeed(bool isAd)
		{
			BattleChapterPlayerData playerData = this.CurrentState.GetPlayerData();
			if (playerData == null)
			{
				return 0;
			}
			int num = playerData.RefreshSkillCount;
			if (isAd)
			{
				num = GameApp.Data.GetDataModule(DataName.AdDataModule).GetWatchTimes(GameConfig.GameEvent_RefreshSkill_AdId);
			}
			return this.CurrentState.GetRefreshSkillSeed(num, isAd);
		}

		public List<GameEventSkillBuildData> GetSkillPool(SkillBuildSourceType sourceType)
		{
			return this.CurrentState.GetSkillPool(sourceType);
		}

		public int GetRandomBox(int seed)
		{
			return this.CurrentState.GetRandomBox(seed);
		}

		public int GetCurrentBoxId()
		{
			return this.CurrentState.GetCurrentBoxId();
		}

		public void SetFixBoxId(int boxId, int seed)
		{
			this.CurrentState.SetFixBoxId(boxId, seed);
		}

		public int GetCurBoxSkillNum()
		{
			return this.CurrentState.GetCurBoxSkillNum();
		}

		public int GetRandomSurprise(int buildId, int seed)
		{
			return this.CurrentState.GetRandomSurprise(buildId, seed);
		}

		public int GetCurrentSurpriseId()
		{
			return this.CurrentState.GetCurrentSurpriseId();
		}

		public void SetCurrentSurpriseId(int surpriseId)
		{
			this.CurrentState.SetCurrentSurpriseId(surpriseId);
		}

		public GameEventAttributesFactory.AttributeBuild RandomAttributeBuildId(int seed)
		{
			return this.CurrentState.RandomAttributeBuildId(seed);
		}

		public List<GameEventSlotTrainFactory.SlotTrainBuild> CreateSlotTrainBuilds(List<GameEventSkillBuildData> skills, int seed)
		{
			return this.CurrentState.CreateSlotTrainBuilds(skills, seed);
		}

		public GameEventSlotTrainFactory.SlotTrainBuild RandomSlotTrain()
		{
			return this.CurrentState.RandomSlotTrain();
		}

		public void CloseMiniGame(MiniGameType miniGameType, object param)
		{
			GameEventPushType gameEventPushType = GameEventPushType.None;
			switch (miniGameType)
			{
			case MiniGameType.MiniSlot:
				gameEventPushType = GameEventPushType.CloseSlotMachine;
				break;
			case MiniGameType.CardFlipping:
				gameEventPushType = GameEventPushType.CloseCardFlipping;
				break;
			case MiniGameType.Turntable:
				gameEventPushType = GameEventPushType.CloseTurntable;
				break;
			case MiniGameType.PaySlot:
				gameEventPushType = GameEventPushType.ClosePaySlot;
				break;
			}
			if (gameEventPushType != GameEventPushType.None)
			{
				this.PushEvent(gameEventPushType, param);
			}
		}

		public XRandom GetGroupRandom()
		{
			return this.CurrentState.GetGroupRandom();
		}

		public int GetEventSizeTypeNum(EventSizeType sizeType)
		{
			return this.CurrentState.GetEventSizeTypeNum(sizeType);
		}

		private bool isInit;

		private StateMachine _stateMachine;

		private List<GameEventSkillBuildData> lastLearnSkills = new List<GameEventSkillBuildData>();
	}
}
