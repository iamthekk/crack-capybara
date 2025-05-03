using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GuildRace_opentimeModelImpl : BaseLocalModelImpl<GuildRace_opentime, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GuildRace_opentime();
		}

		protected override int GetBeanKey(GuildRace_opentime bean)
		{
			return bean.ID;
		}
	}
}
