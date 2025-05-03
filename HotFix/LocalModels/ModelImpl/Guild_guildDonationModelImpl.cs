using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Guild_guildDonationModelImpl : BaseLocalModelImpl<Guild_guildDonation, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Guild_guildDonation();
		}

		protected override int GetBeanKey(Guild_guildDonation bean)
		{
			return bean.ID;
		}
	}
}
