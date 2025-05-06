using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GuildRace_baseRaceModelImpl : BaseLocalModelImpl<GuildRace_baseRace, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GuildRace_baseRace();
		}

		protected override int GetBeanKey(GuildRace_baseRace bean)
		{
			return bean.ID;
		}
	}
}
