using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GuildBOSS_guildBossTaskModelImpl : BaseLocalModelImpl<GuildBOSS_guildBossTask, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GuildBOSS_guildBossTask();
		}

		protected override int GetBeanKey(GuildBOSS_guildBossTask bean)
		{
			return bean.ID;
		}
	}
}
