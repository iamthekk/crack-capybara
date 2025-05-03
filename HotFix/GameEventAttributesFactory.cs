using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using Server;

namespace HotFix
{
	public class GameEventAttributesFactory
	{
		public void Init()
		{
			this.attDic.Clear();
			this.totalWeight = 0;
			IList<Chapter_attributeBuild> allElements = GameApp.Table.GetManager().GetChapter_attributeBuildModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				Chapter_attributeBuild chapter_attributeBuild = allElements[i];
				List<MergeAttributeData> mergeAttributeData = chapter_attributeBuild.attributes.GetMergeAttributeData();
				if (mergeAttributeData != null && mergeAttributeData.Count != 0)
				{
					GameEventAttributesFactory.AttributeBuild attributeBuild = new GameEventAttributesFactory.AttributeBuild(chapter_attributeBuild.id, chapter_attributeBuild.weight, chapter_attributeBuild.attributes);
					this.attDic.Add(allElements[i].id, attributeBuild);
					this.totalWeight += attributeBuild.weight;
				}
			}
		}

		public GameEventAttributesFactory.AttributeBuild RandomAttribute(int seed)
		{
			int num = new XRandom(seed).Range(0, this.totalWeight);
			int num2 = 0;
			foreach (GameEventAttributesFactory.AttributeBuild attributeBuild in this.attDic.Values)
			{
				num2 += attributeBuild.weight;
				if (num < num2)
				{
					return attributeBuild;
				}
			}
			return null;
		}

		private Dictionary<int, GameEventAttributesFactory.AttributeBuild> attDic = new Dictionary<int, GameEventAttributesFactory.AttributeBuild>();

		private int totalWeight;

		public class AttributeBuild
		{
			public int id { get; private set; }

			public int weight { get; private set; }

			public MergeAttributeData attData { get; private set; }

			public Chapter_attributeBuild Config
			{
				get
				{
					if (this._config == null)
					{
						this._config = GameApp.Table.GetManager().GetChapter_attributeBuildModelInstance().GetElementById(this.id);
					}
					return this._config;
				}
			}

			public AttributeBuild(int id, int weight, string atts)
			{
				this.id = id;
				this.weight = weight;
				List<MergeAttributeData> mergeAttributeData = atts.GetMergeAttributeData();
				if (mergeAttributeData != null && mergeAttributeData.Count > 0)
				{
					this.attData = mergeAttributeData[0];
				}
			}

			private Chapter_attributeBuild _config;
		}
	}
}
