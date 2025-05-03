using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Shop_SummonPoolModelImpl : BaseLocalModelImpl<Shop_SummonPool, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Shop_SummonPool();
		}

		protected override int GetBeanKey(Shop_SummonPool bean)
		{
			return bean.id;
		}
	}
}
