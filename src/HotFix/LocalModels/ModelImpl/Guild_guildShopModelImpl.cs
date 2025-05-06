using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Guild_guildShopModelImpl : BaseLocalModelImpl<Guild_guildShop, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Guild_guildShop();
		}

		protected override int GetBeanKey(Guild_guildShop bean)
		{
			return bean.ID;
		}
	}
}
