using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class WorldBoss_RewardModelImpl : BaseLocalModelImpl<WorldBoss_Reward, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new WorldBoss_Reward();
		}

		protected override int GetBeanKey(WorldBoss_Reward bean)
		{
			return bean.ID;
		}
	}
}
