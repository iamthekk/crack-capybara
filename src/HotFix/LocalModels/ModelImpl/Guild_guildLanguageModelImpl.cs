using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Guild_guildLanguageModelImpl : BaseLocalModelImpl<Guild_guildLanguage, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Guild_guildLanguage();
		}

		protected override int GetBeanKey(Guild_guildLanguage bean)
		{
			return bean.ID;
		}
	}
}
