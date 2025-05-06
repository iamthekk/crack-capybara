using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class IAP_GiftPacksModelImpl : BaseLocalModelImpl<IAP_GiftPacks, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new IAP_GiftPacks();
		}

		protected override int GetBeanKey(IAP_GiftPacks bean)
		{
			return bean.id;
		}
	}
}
