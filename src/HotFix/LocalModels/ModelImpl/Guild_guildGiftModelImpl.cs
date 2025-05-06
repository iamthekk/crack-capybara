using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Guild_guildGiftModelImpl : BaseLocalModelImpl<Guild_guildGift, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Guild_guildGift();
		}

		protected override int GetBeanKey(Guild_guildGift bean)
		{
			return bean.ID;
		}
	}
}
