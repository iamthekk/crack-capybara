using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Guild_guildLevelModelImpl : BaseLocalModelImpl<Guild_guildLevel, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Guild_guildLevel();
		}

		protected override int GetBeanKey(Guild_guildLevel bean)
		{
			return bean.ID;
		}
	}
}
