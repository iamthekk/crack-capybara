using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class CrossArenaRewards
	{
		public static CrossArenaRewards Create(int dan)
		{
			CrossArenaRewards crossArenaRewards = new CrossArenaRewards();
			crossArenaRewards.Dan = dan;
			IList<CrossArena_CrossArenaReward> allElements = GameApp.Table.GetManager().GetCrossArena_CrossArenaRewardModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				CrossArena_CrossArenaReward crossArena_CrossArenaReward = allElements[i];
				if (crossArena_CrossArenaReward != null && crossArena_CrossArenaReward.levelID == dan)
				{
					string[] ranking = crossArena_CrossArenaReward.ranking;
					int num;
					int num2;
					if (ranking.Length >= 2 && int.TryParse(ranking[0], out num) && int.TryParse(ranking[1], out num2))
					{
						CrossArenaRankRewards crossArenaRankRewards = new CrossArenaRankRewards();
						crossArenaRankRewards.RankStart = num;
						crossArenaRankRewards.RankEnd = num2;
						crossArenaRankRewards.DailyRewards.AddRange(crossArena_CrossArenaReward.DailyRewards.ToPropDataList());
						crossArenaRankRewards.SeasonRewards.AddRange(crossArena_CrossArenaReward.weeklyRewards.ToPropDataList());
						crossArenaRewards.RankRewards.Add(crossArenaRankRewards);
					}
				}
			}
			crossArenaRewards.RankRewards.Sort(new Comparison<CrossArenaRankRewards>(CrossArenaRankRewards.SortByRank));
			return crossArenaRewards;
		}

		public int Dan;

		public List<CrossArenaRankRewards> RankRewards = new List<CrossArenaRankRewards>();
	}
}
