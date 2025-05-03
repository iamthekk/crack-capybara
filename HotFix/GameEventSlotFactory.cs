using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using Server;

namespace HotFix
{
	public class GameEventSlotFactory
	{
		public void Init()
		{
			ChapterDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			this.sysRandom = new XRandom(dataModule.RandomSeed);
			this.slotBuilds.Clear();
			IList<Chapter_slotBuild> allElements = GameApp.Table.GetManager().GetChapter_slotBuildModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				GameEventSlotFactory.SlotBuild slotBuild = new GameEventSlotFactory.SlotBuild(allElements[i].id);
				this.slotBuilds.Add(slotBuild);
			}
		}

		public GameEventSlotFactory.SlotBuild RandomSlot()
		{
			this.CheckSlotSkillWeight();
			int num = 0;
			for (int i = 0; i < this.slotBuilds.Count; i++)
			{
				GameEventSlotFactory.SlotBuild slotBuild = this.slotBuilds[i];
				num += slotBuild.weight;
			}
			int num2 = this.sysRandom.Range(0, num);
			int num3 = 0;
			for (int j = 0; j < this.slotBuilds.Count; j++)
			{
				GameEventSlotFactory.SlotBuild slotBuild2 = this.slotBuilds[j];
				num3 += slotBuild2.weight;
				if (num2 < num3)
				{
					return slotBuild2;
				}
			}
			return null;
		}

		public List<GameEventSlotFactory.SlotBuild> GetShowIconSlotBuilds()
		{
			List<GameEventSlotFactory.SlotBuild> list = new List<GameEventSlotFactory.SlotBuild>();
			for (int i = 0; i < this.slotBuilds.Count; i++)
			{
				if (this.slotBuilds[i].slotType != SlotType.None)
				{
					list.Add(this.slotBuilds[i]);
				}
			}
			return list;
		}

		private void CheckSlotSkillWeight()
		{
			for (int i = 0; i < this.slotBuilds.Count; i++)
			{
				if (this.slotBuilds[i].IsLegendSkill)
				{
					if (Singleton<GameEventController>.Instance.IsSkillPoolEmpty(SkillBuildSourceType.SlotLegend))
					{
						this.slotBuilds[i].SetWeight(0);
					}
				}
				else if (this.slotBuilds[i].IsNormalSkill && Singleton<GameEventController>.Instance.IsSkillPoolEmpty(SkillBuildSourceType.SlotNormal))
				{
					this.slotBuilds[i].SetWeight(0);
				}
			}
		}

		private List<GameEventSlotFactory.SlotBuild> slotBuilds = new List<GameEventSlotFactory.SlotBuild>();

		private XRandom sysRandom;

		public class SlotBuild
		{
			public int id { get; private set; }

			public int weight { get; private set; }

			public SlotType slotType { get; private set; }

			public int param { get; private set; }

			public Chapter_slotBuild Config
			{
				get
				{
					if (this._config == null)
					{
						this._config = GameApp.Table.GetManager().GetChapter_slotBuildModelInstance().GetElementById(this.id);
					}
					return this._config;
				}
			}

			public SlotBuild(int id)
			{
				this.id = id;
				this.weight = this.Config.weight;
				if (this.Config.param.Length != 0)
				{
					this.slotType = (SlotType)this.Config.param[0];
				}
				if (this.Config.param.Length > 1)
				{
					this.param = this.Config.param[1];
				}
			}

			public bool IsLegendSkill
			{
				get
				{
					return this.slotType == SlotType.Skill && this.param > 0;
				}
			}

			public bool IsNormalSkill
			{
				get
				{
					return this.slotType == SlotType.Skill && this.param == 0;
				}
			}

			public void SetWeight(int w)
			{
				this.weight = w;
			}

			private Chapter_slotBuild _config;
		}
	}
}
