using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using Server;

namespace HotFix
{
	public class GameEventFishingFactory
	{
		public Fishing_fishing baseConfig { get; private set; }

		public int baitNum { get; private set; }

		public void Init()
		{
			this.Clear();
		}

		public FishData RandomFish(FishingEval eval, int seed)
		{
			int num = 100;
			if (eval == FishingEval.Nice)
			{
				num = this.baseConfig.fishUp[0];
			}
			else if (eval == FishingEval.Perfect)
			{
				num = this.baseConfig.fishUp[1];
			}
			RandomWeight<int> randomWeight = new RandomWeight<int>(1);
			foreach (int num2 in this.fishCountDic.Keys)
			{
				int num3 = this.fishCountDic[num2];
				if (num3 != 0)
				{
					int num4 = 100;
					if (this.fishDic[num2].type != 1)
					{
						num4 = num;
					}
					for (int i = 0; i < num3; i++)
					{
						randomWeight.Add(num4, num2);
					}
				}
			}
			int num5 = randomWeight.Dequeue();
			Dictionary<int, int> dictionary = this.fishCountDic;
			int num6 = num5;
			int num7 = dictionary[num6];
			dictionary[num6] = num7 - 1;
			Fishing_fish fishing_fish = this.fishDic[num5];
			float weight = (float)fishing_fish.weight;
			int num8 = new XRandom(seed).Range(fishing_fish.weightFloat[0], fishing_fish.weightFloat[1]);
			int num9 = (int)(weight * ((float)num8 / 100f));
			return new FishData(num5, num9, num8);
		}

		public void AddBait(int num)
		{
			this.baitNum += num;
		}

		public bool UseBait()
		{
			if (this.baitNum > 0)
			{
				int baitNum = this.baitNum;
				this.baitNum = baitNum - 1;
				return true;
			}
			return false;
		}

		public int GetBestRod()
		{
			int num = this.defaultRod;
			List<GameEventItemData> itemsByType = Singleton<GameEventController>.Instance.GetItemsByType(EventItemType.FishRod);
			int num2 = 0;
			for (int i = 0; i < itemsByType.Count; i++)
			{
				int num3;
				if (int.TryParse(itemsByType[i].param, out num3))
				{
					Fishing_fishRod elementById = GameApp.Table.GetManager().GetFishing_fishRodModelInstance().GetElementById(num3);
					if (elementById != null && num2 < elementById.type)
					{
						num = num3;
						num2 = elementById.type;
					}
				}
			}
			return num;
		}

		public void Clear()
		{
			this.fishDic.Clear();
			this.fishCountDic.Clear();
		}

		private int defaultRod;

		private Dictionary<int, Fishing_fish> fishDic = new Dictionary<int, Fishing_fish>();

		private Dictionary<int, int> fishCountDic = new Dictionary<int, int>();
	}
}
