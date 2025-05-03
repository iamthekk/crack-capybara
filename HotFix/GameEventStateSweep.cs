using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class GameEventStateSweep : GameEventStateBase
	{
		public GameEventStateSweep(int id)
			: base(id)
		{
		}

		private ChapterSweepDataModule sweepDataModule
		{
			get
			{
				return GameApp.Data.GetDataModule(DataName.ChapterSweepDataModule);
			}
		}

		public override void OnEnter()
		{
			bool flag = Singleton<EventRecordController>.Instance.IsHaveSweepRecord();
			if (flag)
			{
				this.sweepDataModule.UseSweepRecord();
			}
			AddAttributeDataModule dataModule = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
			HeroDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.HeroDataModule);
			PetDataModule dataModule3 = GameApp.Data.GetDataModule(DataName.PetDataModule);
			this.PlayerData = new BattleChapterPlayerData(dataModule2.MainCardData.m_memberID, dataModule.MemberAttributeData, dataModule.SkillIDs, dataModule3.GetFightPetCardData());
			this.sweepChapter = GameApp.Table.GetManager().GetChapter_chapterModelInstance().GetElementById(this.sweepDataModule.SweepChapterId);
			this.dropDataCtrl = new BattleChapterDropDataCtrl();
			this.poolDataFactory = new GameEventPoolDataFactory();
			this.monsterFactory = new GameEventMonsterFactory();
			this.eventFactory = new GameEventFactory();
			this.poolDataFactory.Init(this.sweepDataModule.RandomSeed, this.sweepChapter.difficultStage, 0, 0, this.sweepDataModule.ServerEventMap, this.sweepDataModule.AddActivityMap);
			this.monsterFactory.Init(this.sweepDataModule.RandomSeed);
			if (flag)
			{
				this.UserDropRecord();
			}
		}

		public override void OnUpdate(float deltaTime)
		{
		}

		public override void OnExit()
		{
			this.dropDataCtrl.Clear();
			this.eventFactory.DeInit();
			this.dropDataCtrl = null;
			this.eventFactory = null;
			this.monsterFactory = null;
			this.poolDataFactory = null;
		}

		public override void StartEvent()
		{
			this.eventFactory.Init(this.sweepChapter.journeyStage, this.poolDataFactory);
		}

		private void UserDropRecord()
		{
			List<BattleChapterDropData> dropList = Singleton<EventRecordController>.Instance.SweepRecord.GetDropList();
			for (int i = 0; i < dropList.Count; i++)
			{
				this.dropDataCtrl.AddDrop(dropList[i]);
			}
		}

		private Chapter_chapter sweepChapter;
	}
}
