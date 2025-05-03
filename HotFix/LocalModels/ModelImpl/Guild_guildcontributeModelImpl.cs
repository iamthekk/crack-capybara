using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Guild_guildcontributeModelImpl : BaseLocalModelImpl<Guild_guildcontribute, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Guild_guildcontribute();
		}

		protected override int GetBeanKey(Guild_guildcontribute bean)
		{
			return bean.Times;
		}
	}
}
