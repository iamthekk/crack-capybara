using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class IntegralShop_goodsModelImpl : BaseLocalModelImpl<IntegralShop_goods, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new IntegralShop_goods();
		}

		protected override int GetBeanKey(IntegralShop_goods bean)
		{
			return bean.ID;
		}
	}
}
