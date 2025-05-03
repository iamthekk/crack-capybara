using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Avatar_SceneSkinModelImpl : BaseLocalModelImpl<Avatar_SceneSkin, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Avatar_SceneSkin();
		}

		protected override int GetBeanKey(Avatar_SceneSkin bean)
		{
			return bean.id;
		}
	}
}
