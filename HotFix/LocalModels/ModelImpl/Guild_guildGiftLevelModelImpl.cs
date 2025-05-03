using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Guild_guildGiftLevelModelImpl : BaseLocalModelImpl<Guild_guildGiftLevel, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Guild_guildGiftLevel();
		}

		protected override int GetBeanKey(Guild_guildGiftLevel bean)
		{
			return bean.ID;
		}
	}
}
