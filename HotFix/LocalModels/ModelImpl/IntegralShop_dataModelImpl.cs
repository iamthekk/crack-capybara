using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class IntegralShop_dataModelImpl : BaseLocalModelImpl<IntegralShop_data, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new IntegralShop_data();
		}

		protected override int GetBeanKey(IntegralShop_data bean)
		{
			return bean.ID;
		}
	}
}
