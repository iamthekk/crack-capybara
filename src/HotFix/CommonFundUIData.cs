using System;
using System.Collections.Generic;

namespace HotFix
{
	public class CommonFundUIData
	{
		public static int Sort(CommonFundUIData a, CommonFundUIData b)
		{
			int num = a.IsLoopReward.CompareTo(b.IsLoopReward);
			if (num == 0)
			{
				num = a.Score.CompareTo(b.Score);
			}
			return num;
		}

		public int ConfigId;

		public int Score;

		public int PreviousScore;

		public int NextScore;

		public bool IsLoopReward;

		public int LoopRewardLimit;

		public List<ItemData> FreeRewards = new List<ItemData>();

		public List<ItemData> PayRewards = new List<ItemData>();
	}
}
