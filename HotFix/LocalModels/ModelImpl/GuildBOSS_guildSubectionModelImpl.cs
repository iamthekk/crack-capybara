using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GuildBOSS_guildSubectionModelImpl : BaseLocalModelImpl<GuildBOSS_guildSubection, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GuildBOSS_guildSubection();
		}

		protected override int GetBeanKey(GuildBOSS_guildSubection bean)
		{
			return bean.ID;
		}
	}
}
