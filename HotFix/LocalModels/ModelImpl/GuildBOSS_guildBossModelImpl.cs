using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GuildBOSS_guildBossModelImpl : BaseLocalModelImpl<GuildBOSS_guildBoss, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GuildBOSS_guildBoss();
		}

		protected override int GetBeanKey(GuildBOSS_guildBoss bean)
		{
			return bean.ID;
		}
	}
}
