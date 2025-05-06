using System;
using System.Collections.Generic;

namespace HotFix
{
	public class CrossArenaRankRewards
	{
		public static int SortByRank(CrossArenaRankRewards x, CrossArenaRankRewards y)
		{
			return x.RankStart.CompareTo(y.RankStart);
		}

		public int RankStart;

		public int RankEnd;

		public List<PropData> DailyRewards = new List<PropData>();

		public List<PropData> SeasonRewards = new List<PropData>();
	}
}
