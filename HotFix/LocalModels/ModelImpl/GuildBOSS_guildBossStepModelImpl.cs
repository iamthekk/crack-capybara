using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GuildBOSS_guildBossStepModelImpl : BaseLocalModelImpl<GuildBOSS_guildBossStep, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GuildBOSS_guildBossStep();
		}

		protected override int GetBeanKey(GuildBOSS_guildBossStep bean)
		{
			return bean.BossLevel;
		}
	}
}
