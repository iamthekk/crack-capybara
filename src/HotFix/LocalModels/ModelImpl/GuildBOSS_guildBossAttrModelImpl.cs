using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GuildBOSS_guildBossAttrModelImpl : BaseLocalModelImpl<GuildBOSS_guildBossAttr, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GuildBOSS_guildBossAttr();
		}

		protected override int GetBeanKey(GuildBOSS_guildBossAttr bean)
		{
			return bean.id;
		}
	}
}
