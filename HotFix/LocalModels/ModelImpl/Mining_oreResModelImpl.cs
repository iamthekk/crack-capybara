using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Mining_oreResModelImpl : BaseLocalModelImpl<Mining_oreRes, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Mining_oreRes();
		}

		protected override int GetBeanKey(Mining_oreRes bean)
		{
			return bean.id;
		}
	}
}
