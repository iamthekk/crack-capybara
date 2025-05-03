using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Mining_oreQualityModelImpl : BaseLocalModelImpl<Mining_oreQuality, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Mining_oreQuality();
		}

		protected override int GetBeanKey(Mining_oreQuality bean)
		{
			return bean.id;
		}
	}
}
