using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GuildBOSS_guildBossSeasonRewardModelImpl : BaseLocalModelImpl<GuildBOSS_guildBossSeasonReward, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GuildBOSS_guildBossSeasonReward();
		}

		protected override int GetBeanKey(GuildBOSS_guildBossSeasonReward bean)
		{
			return bean.ID;
		}
	}
}
