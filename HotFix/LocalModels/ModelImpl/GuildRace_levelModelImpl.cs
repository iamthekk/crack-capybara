using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GuildRace_levelModelImpl : BaseLocalModelImpl<GuildRace_level, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GuildRace_level();
		}

		protected override int GetBeanKey(GuildRace_level bean)
		{
			return bean.ID;
		}
	}
}
