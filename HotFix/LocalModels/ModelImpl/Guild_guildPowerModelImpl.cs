using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Guild_guildPowerModelImpl : BaseLocalModelImpl<Guild_guildPower, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Guild_guildPower();
		}

		protected override int GetBeanKey(Guild_guildPower bean)
		{
			return bean.ID;
		}
	}
}
