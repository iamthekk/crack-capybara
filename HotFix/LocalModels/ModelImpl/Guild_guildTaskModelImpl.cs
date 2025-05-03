using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Guild_guildTaskModelImpl : BaseLocalModelImpl<Guild_guildTask, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Guild_guildTask();
		}

		protected override int GetBeanKey(Guild_guildTask bean)
		{
			return bean.ID;
		}
	}
}
