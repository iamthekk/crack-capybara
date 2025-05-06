using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Avatar_SkinModelImpl : BaseLocalModelImpl<Avatar_Skin, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Avatar_Skin();
		}

		protected override int GetBeanKey(Avatar_Skin bean)
		{
			return bean.id;
		}
	}
}
