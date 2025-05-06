using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class WorldBoss_WorldBossBoxModelImpl : BaseLocalModelImpl<WorldBoss_WorldBossBox, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new WorldBoss_WorldBossBox();
		}

		protected override int GetBeanKey(WorldBoss_WorldBossBox bean)
		{
			return bean.ID;
		}
	}
}
