using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class IAP_DiamondPacksModelImpl : BaseLocalModelImpl<IAP_DiamondPacks, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new IAP_DiamondPacks();
		}

		protected override int GetBeanKey(IAP_DiamondPacks bean)
		{
			return bean.id;
		}
	}
}
