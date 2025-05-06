using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GuildBOSS_guildBossMonsterModelImpl : BaseLocalModelImpl<GuildBOSS_guildBossMonster, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GuildBOSS_guildBossMonster();
		}

		protected override int GetBeanKey(GuildBOSS_guildBossMonster bean)
		{
			return bean.id;
		}
	}
}
