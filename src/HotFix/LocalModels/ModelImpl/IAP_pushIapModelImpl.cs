using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class IAP_pushIapModelImpl : BaseLocalModelImpl<IAP_pushIap, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new IAP_pushIap();
		}

		protected override int GetBeanKey(IAP_pushIap bean)
		{
			return bean.id;
		}
	}
}
