using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Guild_guildStyleModelImpl : BaseLocalModelImpl<Guild_guildStyle, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Guild_guildStyle();
		}

		protected override int GetBeanKey(Guild_guildStyle bean)
		{
			return bean.ID;
		}
	}
}
