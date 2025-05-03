using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Guild_guildEventModelImpl : BaseLocalModelImpl<Guild_guildEvent, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Guild_guildEvent();
		}

		protected override int GetBeanKey(Guild_guildEvent bean)
		{
			return bean.id;
		}
	}
}
