using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using Server;

namespace HotFix
{
	public class GameEventSurpriseBuildFactory
	{
		public void Init()
		{
			this.surpriseDic.Clear();
			IList<Chapter_surpriseBuild> allElements = GameApp.Table.GetManager().GetChapter_surpriseBuildModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				Chapter_surpriseBuild chapter_surpriseBuild = allElements[i];
				GameEventSurpriseBuildFactory.SurpriseBuild surpriseBuild = new GameEventSurpriseBuildFactory.SurpriseBuild(chapter_surpriseBuild.id, chapter_surpriseBuild.weight);
				List<GameEventSurpriseBuildFactory.SurpriseBuild> list;
				if (this.surpriseDic.TryGetValue(chapter_surpriseBuild.buildId, out list))
				{
					list.Add(surpriseBuild);
				}
				else
				{
					List<GameEventSurpriseBuildFactory.SurpriseBuild> list2 = new List<GameEventSurpriseBuildFactory.SurpriseBuild>();
					list2.Add(surpriseBuild);
					this.surpriseDic.Add(chapter_surpriseBuild.buildId, list2);
				}
			}
		}

		public int RandomSurprise(int buildId, int seed)
		{
			this.currentSurpriseId = 0;
			int num = 0;
			List<GameEventSurpriseBuildFactory.SurpriseBuild> list;
			if (this.surpriseDic.TryGetValue(buildId, out list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					num += list[i].weight;
				}
				int num2 = new XRandom(seed).Range(0, num);
				int num3 = 0;
				for (int j = 0; j < list.Count; j++)
				{
					GameEventSurpriseBuildFactory.SurpriseBuild surpriseBuild = list[j];
					num3 += surpriseBuild.weight;
					if (num2 < num3)
					{
						this.currentSurpriseId = surpriseBuild.id;
						break;
					}
				}
			}
			return this.currentSurpriseId;
		}

		public int GetCurrentSurpriseId()
		{
			return this.currentSurpriseId;
		}

		public void SetCurrentSurpriseId(int surpriseId)
		{
			this.currentSurpriseId = surpriseId;
		}

		private Dictionary<int, List<GameEventSurpriseBuildFactory.SurpriseBuild>> surpriseDic = new Dictionary<int, List<GameEventSurpriseBuildFactory.SurpriseBuild>>();

		private int currentSurpriseId;

		public class SurpriseBuild
		{
			public int id { get; private set; }

			public int weight { get; private set; }

			public Chapter_surpriseBuild Config
			{
				get
				{
					if (this._config == null)
					{
						this._config = GameApp.Table.GetManager().GetChapter_surpriseBuildModelInstance().GetElementById(this.id);
					}
					return this._config;
				}
			}

			public SurpriseBuild(int id, int weight)
			{
				this.id = id;
				this.weight = weight;
			}

			private Chapter_surpriseBuild _config;
		}
	}
}
