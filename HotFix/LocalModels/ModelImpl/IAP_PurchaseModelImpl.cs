using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class IAP_PurchaseModelImpl : BaseLocalModelImpl<IAP_Purchase, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new IAP_Purchase();
		}

		protected override int GetBeanKey(IAP_Purchase bean)
		{
			return bean.id;
		}
	}
}
