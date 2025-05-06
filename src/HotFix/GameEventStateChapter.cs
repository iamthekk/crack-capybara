using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;

namespace HotFix
{
	public class GameEventStateChapter : GameEventStateBase
	{
		private ChapterDataModule chapterModule
		{
			get
			{
				return GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			}
		}

		public GameEventStateChapter(int id)
			: base(id)
		{
		}

		public override void OnEnter()
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Game_BattleChangeAttribute, new HandlerEvent(this.OnEventBattleChangeAttribute));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_GameOverRefreshAttr, new HandlerEvent(this.OnEventGameOver));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_EventGroup_End, new HandlerEvent(this.OnEventGroupEnd));
			AddAttributeDataModule dataModule = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
			HeroDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.HeroDataModule);
			PetDataModule dataModule3 = GameApp.Data.GetDataModule(DataName.PetDataModule);
			int[] difficultStage = this.chapterModule.CurrentChapter.Config.difficultStage;
			this.PlayerData = new BattleChapterPlayerData(dataModule2.MainCardData.m_memberID, dataModule.MemberAttributeData, dataModule.SkillIDs, dataModule3.GetFightPetCardData());
			int firstEventWithSkill = GameApp.Data.GetDataModule(DataName.ChapterDataModule).CurrentChapter.firstEventWithSkill;
			int num = this.PlayerData.AttributeData.GetSkillCountByBegin.FloorToInt();
			this.isHaveRecord = Singleton<EventRecordController>.Instance.IsHaveChapterRecord();
			if (this.isHaveRecord)
			{
				this.chapterModule.UseGlobalRecord();
			}
			this.dropDataCtrl = new BattleChapterDropDataCtrl();
			this.poolDataFactory = new GameEventPoolDataFactory();
			this.monsterFactory = new GameEventMonsterFactory();
			this.skillBuildPool = new GameEventSkillBuildFactory();
			this.boxBuildBuildPool = new GameEventBoxBuildFactory();
			this.surpriseBuildPool = new GameEventSurpriseBuildFactory();
			this.attributeBuildPool = new GameEventAttributesFactory();
			this.slotTrainPool = new GameEventSlotTrainFactory();
			this.itemFactory = new GameEventItemFactory();
			this.fishingFactory = new GameEventFishingFactory();
			this.eventFactory = new GameEventFactory();
			this.monsterFactory.Init(this.chapterModule.RandomSeed);
			this.skillBuildPool.Init(this.chapterModule.RandomSeed);
			this.boxBuildBuildPool.Init();
			this.surpriseBuildPool.Init();
			this.attributeBuildPool.Init();
			this.slotTrainPool.Init();
			this.poolDataFactory.Init(this.chapterModule.RandomSeed, difficultStage, num, firstEventWithSkill, this.chapterModule.ServerEventMap, this.chapterModule.AddActivityMap);
			this.itemFactory.Init();
			this.fishingFactory.Init();
			if (this.isHaveRecord)
			{
				this.UserPlayerRecord();
			}
		}

		public override void OnUpdate(float deltaTime)
		{
		}

		public override void OnExit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Game_BattleChangeAttribute, new HandlerEvent(this.OnEventBattleChangeAttribute));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_GameOverRefreshAttr, new HandlerEvent(this.OnEventGameOver));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_EventGroup_End, new HandlerEvent(this.OnEventGroupEnd));
			this.PlayerData.ClearData();
			this.PlayerData = null;
			this.eventFactory.DeInit();
			this.eventFactory = null;
			this.poolDataFactory = null;
			this.monsterFactory = null;
			this.skillBuildPool = null;
			this.boxBuildBuildPool = null;
			this.surpriseBuildPool = null;
			this.attributeBuildPool = null;
			this.slotTrainPool = null;
			this.itemFactory = null;
			this.fishingFactory.Clear();
			this.fishingFactory = null;
		}

		public override void StartEvent()
		{
			if (this.isHaveRecord)
			{
				this.UserSkillRecord();
				this.UseEventItemRecord();
			}
			int totalStage = this.chapterModule.CurrentChapter.TotalStage;
			this.eventFactory.Init(totalStage, this.poolDataFactory);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_StartEvent, null);
			if (!Singleton<EventRecordController>.Instance.IsHaveChapterRecord())
			{
				Singleton<EventRecordController>.Instance.SavePlayerData();
			}
		}

		public override void ContinueEvent()
		{
			this.eventFactory.ContinueEvent();
		}

		private void OnEventBattleChangeAttribute(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsGameChangeAttribute eventArgsGameChangeAttribute = eventArgs as EventArgsGameChangeAttribute;
			if (eventArgsGameChangeAttribute != null)
			{
				this.PlayerData.BattleChangeAttribute(eventArgsGameChangeAttribute.changeData);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_RefreshAttribute, null);
			}
		}

		private void OnEventGameOver(object sender, int type, BaseEventArgs eventArgs)
		{
			this.PlayerData.SetEventAttributes();
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_RefreshAttribute, null);
		}

		private void OnEventGroupEnd(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsInt eventArgsInt = eventArgs as EventArgsInt;
			if (eventArgsInt != null)
			{
				base.CheckEventItemRemove(eventArgsInt.Value);
			}
		}

		private void UserPlayerRecord()
		{
			EventRecordPlayerData playerRecord = Singleton<EventRecordController>.Instance.PlayerRecord;
			this.PlayerData.UserRecord(playerRecord);
			List<BattleChapterDropData> battleChapterDropList = playerRecord.GetBattleChapterDropList();
			for (int i = 0; i < battleChapterDropList.Count; i++)
			{
				this.dropDataCtrl.AddDrop(battleChapterDropList[i]);
			}
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_RefreshAttribute, null);
		}

		private void UserSkillRecord()
		{
			List<int> skillBuildIdList = Singleton<EventRecordController>.Instance.PlayerRecord.GetSkillBuildIdList();
			for (int i = 0; i < skillBuildIdList.Count; i++)
			{
				GameEventSkillBuildData skillByID = this.skillBuildPool.GetSkillByID(skillBuildIdList[i]);
				this.PlayerData.AddRecoverSkillBuild(skillByID);
				this.skillBuildPool.SelectSkill(skillByID, false);
				this.skillBuildPool.RecordRemoveLowLevelSkill(skillByID);
			}
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_RefreshAttribute, null);
		}

		private void UseEventItemRecord()
		{
			List<GameEventItemData> eventItemList = Singleton<EventRecordController>.Instance.PlayerRecord.GetEventItemList();
			this.itemFactory.AddRecordItems(eventItemList);
		}

		private bool isHaveRecord;
	}
}
