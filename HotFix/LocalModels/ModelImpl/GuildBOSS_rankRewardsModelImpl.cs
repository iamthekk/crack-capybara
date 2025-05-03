using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GuildBOSS_rankRewardsModelImpl : BaseLocalModelImpl<GuildBOSS_rankRewards, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GuildBOSS_rankRewards();
		}

		protected override int GetBeanKey(GuildBOSS_rankRewards bean)
		{
			return bean.ID;
		}
	}
}
