using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Guild_guilddairyModelImpl : BaseLocalModelImpl<Guild_guilddairy, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Guild_guilddairy();
		}

		protected override int GetBeanKey(Guild_guilddairy bean)
		{
			return bean.dairy_id;
		}
	}
}
