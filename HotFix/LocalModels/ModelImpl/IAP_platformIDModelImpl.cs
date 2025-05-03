using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class IAP_platformIDModelImpl : BaseLocalModelImpl<IAP_platformID, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new IAP_platformID();
		}

		protected override int GetBeanKey(IAP_platformID bean)
		{
			return bean.id;
		}
	}
}
