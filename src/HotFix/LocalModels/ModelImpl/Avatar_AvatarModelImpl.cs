using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Avatar_AvatarModelImpl : BaseLocalModelImpl<Avatar_Avatar, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Avatar_Avatar();
		}

		protected override int GetBeanKey(Avatar_Avatar bean)
		{
			return bean.id;
		}
	}
}
