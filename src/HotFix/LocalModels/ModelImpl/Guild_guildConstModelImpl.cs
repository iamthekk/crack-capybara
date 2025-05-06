using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Guild_guildConstModelImpl : BaseLocalModelImpl<Guild_guildConst, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Guild_guildConst();
		}

		protected override int GetBeanKey(Guild_guildConst bean)
		{
			return bean.ID;
		}
	}
}
