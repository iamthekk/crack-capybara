using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class WorldBoss_RankRewardModelImpl : BaseLocalModelImpl<WorldBoss_RankReward, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new WorldBoss_RankReward();
		}

		protected override int GetBeanKey(WorldBoss_RankReward bean)
		{
			return bean.ID;
		}
	}
}
