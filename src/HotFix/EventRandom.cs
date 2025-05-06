using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using Server;

namespace HotFix
{
	public class EventRandom
	{
		public EventRandom(int seed)
		{
			this.systemRandom = new XRandom(seed);
		}

		public void Init(int[] events)
		{
			foreach (int num in events)
			{
				Chapter_eventRes elementById = GameApp.Table.GetManager().GetChapter_eventResModelInstance().GetElementById(num);
				if (elementById == null)
				{
					HLog.LogError(string.Format("Table [Chapter_eventRes] not found id={0}", num));
				}
				else
				{
					this.ids.Add(num);
					this.itemDic.Add(num, elementById.items);
					this.weights.Add(elementById.weight);
					this.itemWeights.Add(elementById.itemWeight);
					this.realWeights.Add(elementById.weight);
				}
			}
		}

		public int GetRandom()
		{
			int num = this.CalcTotalWeight();
			if (num == 0)
			{
				this.randomedIds.Clear();
				num = this.CalcTotalWeight();
			}
			int num2 = this.systemRandom.Range(0, num);
			int num3 = 0;
			for (int i = 0; i < this.realWeights.Count; i++)
			{
				num3 += this.realWeights[i];
				if (num2 < num3)
				{
					if (this.randomedIds.Count == this.ids.Count - 1)
					{
						this.randomedIds.Clear();
					}
					this.randomedIds.Add(this.ids[i]);
					return this.ids[i];
				}
			}
			return this.ids[0];
		}

		private int CalcTotalWeight()
		{
			int num = 0;
			for (int i = 0; i < this.ids.Count; i++)
			{
				int num2 = this.ids[i];
				if (this.randomedIds.Contains(num2))
				{
					this.realWeights[i] = 0;
				}
				else if (this.itemDic[num2].Length != 0)
				{
					bool flag = Singleton<GameEventController>.Instance.IsItemsActiveEvent(this.itemDic[num2]);
					this.realWeights[i] = (flag ? this.itemWeights[i] : this.weights[i]);
				}
				else
				{
					this.realWeights[i] = this.weights[i];
				}
				num += this.realWeights[i];
			}
			return num;
		}

		private List<int> ids = new List<int>();

		private List<int> randomedIds = new List<int>();

		private List<int> weights = new List<int>();

		private List<int> itemWeights = new List<int>();

		private List<int> realWeights = new List<int>();

		private Dictionary<int, int[]> itemDic = new Dictionary<int, int[]>();

		private readonly XRandom systemRandom;
	}
}
