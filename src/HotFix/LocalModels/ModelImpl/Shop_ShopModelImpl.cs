using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Shop_ShopModelImpl : BaseLocalModelImpl<Shop_Shop, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Shop_Shop();
		}

		protected override int GetBeanKey(Shop_Shop bean)
		{
			return bean.id;
		}
	}
}
