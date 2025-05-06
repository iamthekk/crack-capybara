using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class WorldBoss_SubsectionModelImpl : BaseLocalModelImpl<WorldBoss_Subsection, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new WorldBoss_Subsection();
		}

		protected override int GetBeanKey(WorldBoss_Subsection bean)
		{
			return bean.ID;
		}
	}
}
