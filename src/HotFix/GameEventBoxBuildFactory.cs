using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using Server;

namespace HotFix
{
	public class GameEventBoxBuildFactory
	{
		public void Init()
		{
			this.boxDic.Clear();
			this.totalWeight = 0;
			ChapterDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			string[] boxBuild = dataModule.CurrentChapter.Config.boxBuild;
			for (int i = 0; i < boxBuild.Length; i++)
			{
				string[] array = boxBuild[i].Split(',', StringSplitOptions.None);
				if (array.Length >= 2)
				{
					int num;
					int num2;
					if (int.TryParse(array[0], out num) && int.TryParse(array[1], out num2))
					{
						GameEventBoxBuildFactory.BoxBuild boxBuild2 = new GameEventBoxBuildFactory.BoxBuild(num, num2);
						this.boxDic.Add(num, boxBuild2);
						this.totalWeight += num2;
					}
				}
				else
				{
					HLog.LogError(string.Format("Chapter {0}: boxBuild format error, boxBuild={1}", dataModule.CurrentChapter.id, boxBuild[i]));
				}
			}
		}

		public int RandomBox(int seed)
		{
			this.currentBoxId = 0;
			int num = new XRandom(seed).Range(0, this.totalWeight);
			int num2 = 0;
			foreach (GameEventBoxBuildFactory.BoxBuild boxBuild in this.boxDic.Values)
			{
				num2 += boxBuild.weight;
				if (num < num2)
				{
					this.currentBoxId = boxBuild.id;
					this.curBoxSkillNum = this.RandomSkillNum(boxBuild, seed);
					break;
				}
			}
			return this.currentBoxId;
		}

		public int RandomSkillNum(GameEventBoxBuildFactory.BoxBuild build, int seed)
		{
			int num = 0;
			Dictionary<int, int> numWeightDic = build.GetNumWeightDic();
			foreach (int num2 in numWeightDic.Values)
			{
				num += num2;
			}
			int num3 = new XRandom(seed).Range(0, num);
			int num4 = 0;
			foreach (int num5 in numWeightDic.Keys)
			{
				num4 += numWeightDic[num5];
				if (num3 < num4)
				{
					return num5;
				}
			}
			return 1;
		}

		public int GetCurrentBoxId()
		{
			return this.currentBoxId;
		}

		public void SetFixBoxId(int boxId, int seed)
		{
			this.currentBoxId = boxId;
			GameEventBoxBuildFactory.BoxBuild boxBuild;
			if (this.boxDic.TryGetValue(boxId, out boxBuild))
			{
				this.curBoxSkillNum = this.RandomSkillNum(boxBuild, seed);
				return;
			}
			HLog.LogError(string.Format("Not found box id={0}", boxId));
		}

		public int GetCurBoxSkillNum()
		{
			return this.curBoxSkillNum;
		}

		private Dictionary<int, GameEventBoxBuildFactory.BoxBuild> boxDic = new Dictionary<int, GameEventBoxBuildFactory.BoxBuild>();

		private int totalWeight;

		private int currentBoxId;

		private int curBoxSkillNum;

		public class BoxBuild
		{
			public int id { get; private set; }

			public int weight { get; private set; }

			public Chapter_boxBuild Config
			{
				get
				{
					if (this._config == null)
					{
						this._config = GameApp.Table.GetManager().GetChapter_boxBuildModelInstance().GetElementById(this.id);
					}
					return this._config;
				}
			}

			public BoxBuild(int id, int weight)
			{
				this.id = id;
				this.weight = weight;
				this.numWeightDic.Clear();
				for (int i = 0; i < this.Config.skillNumWeight.Length; i++)
				{
					string[] array = this.Config.skillNumWeight[i].Split(',', StringSplitOptions.None);
					if (array.Length == 2)
					{
						int num;
						int num2;
						if (int.TryParse(array[0], out num) && int.TryParse(array[1], out num2))
						{
							this.numWeightDic.Add(num, num2);
						}
					}
					else
					{
						HLog.LogError(string.Format("Table box 'skillNumWeight' length error, id={0}", this.Config.id));
					}
				}
			}

			public Dictionary<int, int> GetNumWeightDic()
			{
				return this.numWeightDic;
			}

			private Dictionary<int, int> numWeightDic = new Dictionary<int, int>();

			private Chapter_boxBuild _config;
		}
	}
}
