using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Shop_ShopSellModelImpl : BaseLocalModelImpl<Shop_ShopSell, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Shop_ShopSell();
		}

		protected override int GetBeanKey(Shop_ShopSell bean)
		{
			return bean.id;
		}
	}
}
