using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Shop_AdModelImpl : BaseLocalModelImpl<Shop_Ad, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Shop_Ad();
		}

		protected override int GetBeanKey(Shop_Ad bean)
		{
			return bean.id;
		}
	}
}
