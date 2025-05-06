using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class IAP_PushPacksModelImpl : BaseLocalModelImpl<IAP_PushPacks, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new IAP_PushPacks();
		}

		protected override int GetBeanKey(IAP_PushPacks bean)
		{
			return bean.id;
		}
	}
}
