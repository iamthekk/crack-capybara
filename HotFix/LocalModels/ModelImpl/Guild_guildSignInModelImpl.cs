using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Guild_guildSignInModelImpl : BaseLocalModelImpl<Guild_guildSignIn, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Guild_guildSignIn();
		}

		protected override int GetBeanKey(Guild_guildSignIn bean)
		{
			return bean.ID;
		}
	}
}
