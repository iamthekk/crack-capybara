using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Shop_ShopActivityModelImpl : BaseLocalModelImpl<Shop_ShopActivity, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Shop_ShopActivity();
		}

		protected override int GetBeanKey(Shop_ShopActivity bean)
		{
			return bean.id;
		}
	}
}
