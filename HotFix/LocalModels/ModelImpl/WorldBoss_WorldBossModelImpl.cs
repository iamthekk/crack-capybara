using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class WorldBoss_WorldBossModelImpl : BaseLocalModelImpl<WorldBoss_WorldBoss, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new WorldBoss_WorldBoss();
		}

		protected override int GetBeanKey(WorldBoss_WorldBoss bean)
		{
			return bean.id;
		}
	}
}
