using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic;
using LocalModels.Bean;
using Server;

namespace HotFix
{
	public class GameEventSlotTrainFactory
	{
		public static int SkillCount
		{
			get
			{
				return GameEventSlotTrainFactory._skillCount;
			}
		}

		public void Init()
		{
			int num = 0;
			IList<ChapterMiniGame_slotTrainBuild> allElements = GameApp.Table.GetManager().GetChapterMiniGame_slotTrainBuildModelInstance().GetAllElements();
			new List<ChapterMiniGame_slotTrainBuild>();
			for (int i = 0; i < allElements.Count; i++)
			{
				ChapterMiniGame_slotTrainBuild chapterMiniGame_slotTrainBuild = allElements[i];
				if (chapterMiniGame_slotTrainBuild.param.Length == 0 || chapterMiniGame_slotTrainBuild.param[0] != 5)
				{
					num++;
				}
			}
			GameEventSlotTrainFactory._skillCount = Utility.Math.Clamp(12 - num, 0, 12);
		}

		public List<GameEventSlotTrainFactory.SlotTrainBuild> CreateBuilds(List<GameEventSkillBuildData> skills, int seed)
		{
			this.xRandom = new XRandom(seed);
			this.slotTrainBuilds.Clear();
			IList<ChapterMiniGame_slotTrainBuild> allElements = GameApp.Table.GetManager().GetChapterMiniGame_slotTrainBuildModelInstance().GetAllElements();
			List<ChapterMiniGame_slotTrainBuild> list = new List<ChapterMiniGame_slotTrainBuild>();
			for (int i = 0; i < allElements.Count; i++)
			{
				ChapterMiniGame_slotTrainBuild chapterMiniGame_slotTrainBuild = allElements[i];
				if (chapterMiniGame_slotTrainBuild.param.Length != 0 && chapterMiniGame_slotTrainBuild.param[0] == 5)
				{
					list.Add(chapterMiniGame_slotTrainBuild);
				}
				else
				{
					GameEventSlotTrainFactory.SlotTrainBuild slotTrainBuild = new GameEventSlotTrainFactory.SlotTrainBuild(chapterMiniGame_slotTrainBuild.id);
					this.slotTrainBuilds.Add(slotTrainBuild);
				}
			}
			for (int j = 0; j < skills.Count; j++)
			{
				int quality = (int)skills[j].quality;
				for (int k = 0; k < list.Count; k++)
				{
					ChapterMiniGame_slotTrainBuild chapterMiniGame_slotTrainBuild2 = list[k];
					if (chapterMiniGame_slotTrainBuild2.param.Length > 1 && chapterMiniGame_slotTrainBuild2.param[1] == quality)
					{
						GameEventSlotTrainFactory.SlotTrainBuild slotTrainBuild2 = new GameEventSlotTrainFactory.SlotTrainBuild(chapterMiniGame_slotTrainBuild2.id);
						slotTrainBuild2.SetSkillData(skills[j]);
						this.slotTrainBuilds.Add(slotTrainBuild2);
						break;
					}
				}
			}
			this.slotTrainBuilds = this.RandomSort(this.slotTrainBuilds);
			return this.slotTrainBuilds;
		}

		public GameEventSlotTrainFactory.SlotTrainBuild RandomSlotTrain()
		{
			if (this.xRandom == null)
			{
				return null;
			}
			int num = 0;
			for (int i = 0; i < this.slotTrainBuilds.Count; i++)
			{
				GameEventSlotTrainFactory.SlotTrainBuild slotTrainBuild = this.slotTrainBuilds[i];
				num += slotTrainBuild.weight;
			}
			int num2 = this.xRandom.Range(0, num);
			int num3 = 0;
			for (int j = 0; j < this.slotTrainBuilds.Count; j++)
			{
				GameEventSlotTrainFactory.SlotTrainBuild slotTrainBuild2 = this.slotTrainBuilds[j];
				num3 += slotTrainBuild2.weight;
				if (num2 < num3)
				{
					slotTrainBuild2.SetWeight(0);
					return slotTrainBuild2;
				}
			}
			return null;
		}

		private List<GameEventSlotTrainFactory.SlotTrainBuild> RandomSort(List<GameEventSlotTrainFactory.SlotTrainBuild> list)
		{
			List<GameEventSlotTrainFactory.SlotTrainBuild> list2 = new List<GameEventSlotTrainFactory.SlotTrainBuild>();
			while (list.Count > 0)
			{
				int num = this.xRandom.Range(0, list.Count);
				list2.Add(list[num]);
				list.RemoveAt(num);
			}
			return list2;
		}

		private List<GameEventSlotTrainFactory.SlotTrainBuild> slotTrainBuilds = new List<GameEventSlotTrainFactory.SlotTrainBuild>();

		public const int SLOT_TRAIN_REWARD_COUNT = 12;

		private static int _skillCount;

		private XRandom xRandom;

		public class SlotTrainBuild
		{
			public string id { get; private set; }

			public int tableId { get; private set; }

			public int weight { get; private set; }

			public SlotTrainType slotTrainType { get; private set; }

			public int param { get; private set; }

			public GameEventSkillBuildData skillBuild { get; private set; }

			public ChapterMiniGame_slotTrainBuild Config
			{
				get
				{
					if (this._config == null)
					{
						this._config = GameApp.Table.GetManager().GetChapterMiniGame_slotTrainBuildModelInstance().GetElementById(this.tableId);
					}
					return this._config;
				}
			}

			public SlotTrainBuild(int tid)
			{
				this.id = Guid.NewGuid().ToString();
				this.tableId = tid;
				this.weight = this.Config.weight;
				if (this.Config.param.Length != 0)
				{
					this.slotTrainType = (SlotTrainType)this.Config.param[0];
				}
				if (this.Config.param.Length > 1)
				{
					this.param = this.Config.param[1];
				}
			}

			public void SetWeight(int w)
			{
				this.weight = w;
			}

			public void SetSkillData(GameEventSkillBuildData skill)
			{
				this.skillBuild = skill;
			}

			public bool IsSkill
			{
				get
				{
					return this.skillBuild != null;
				}
			}

			private ChapterMiniGame_slotTrainBuild _config;
		}
	}
}
