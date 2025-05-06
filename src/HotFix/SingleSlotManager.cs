using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using Server;

namespace HotFix
{
	public class SingleSlotManager
	{
		public int AngelScore { get; private set; }

		public int DemonScore { get; private set; }

		public SingleSlotManager(int seed, int freeNum)
		{
			foreach (ChapterMiniGame_singleSlot chapterMiniGame_singleSlot in GameApp.Table.GetManager().GetChapterMiniGame_singleSlotElements())
			{
				SingleSlotData singleSlotData = new SingleSlotData(chapterMiniGame_singleSlot);
				this.randomPool.Add(singleSlotData);
			}
			this.xRandom = new XRandom(seed);
			this.PlayNum = new SecureVariable();
			this.PlayNum.UpdateVariable(freeNum);
		}

		public List<SingleSlotData> GetRandomPool()
		{
			return this.randomPool;
		}

		public SingleSlotData RandomResult()
		{
			if (this.PlayNum.mVariable == 0)
			{
				return null;
			}
			this.PlayNum.UpdateVariable(this.PlayNum.mVariable - 1);
			List<int> list = new List<int>();
			for (int i = 0; i < this.randomPool.Count; i++)
			{
				list.Add(this.randomPool[i].weight);
			}
			int weightedRandomSelection = RandUtils.GetWeightedRandomSelection(list, this.xRandom);
			return this.randomPool[weightedRandomSelection];
		}

		public void AddResult(SingleSlotData result)
		{
			if (result == null)
			{
				return;
			}
			if (result.rewardType == SingleSlotRewardType.ANGEL_SCORE_ID)
			{
				this.AngelScore += result.rewardNum;
				return;
			}
			if (result.rewardType == SingleSlotRewardType.DEMON_SCORE_ID)
			{
				this.DemonScore += result.rewardNum;
				return;
			}
			this.PlayNum.UpdateVariable(this.PlayNum.mVariable + result.rewardNum);
		}

		public readonly SecureVariable PlayNum;

		private List<SingleSlotData> randomPool = new List<SingleSlotData>();

		private XRandom xRandom;
	}
}
