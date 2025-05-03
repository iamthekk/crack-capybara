using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Mining_miningBaseModelImpl : BaseLocalModelImpl<Mining_miningBase, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Mining_miningBase();
		}

		protected override int GetBeanKey(Mining_miningBase bean)
		{
			return bean.id;
		}
	}
}
