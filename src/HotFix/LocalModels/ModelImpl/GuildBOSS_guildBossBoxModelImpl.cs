using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GuildBOSS_guildBossBoxModelImpl : BaseLocalModelImpl<GuildBOSS_guildBossBox, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GuildBOSS_guildBossBox();
		}

		protected override int GetBeanKey(GuildBOSS_guildBossBox bean)
		{
			return bean.ID;
		}
	}
}
