using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Quality_petQualityModelImpl : BaseLocalModelImpl<Quality_petQuality, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Quality_petQuality();
		}

		protected override int GetBeanKey(Quality_petQuality bean)
		{
			return bean.id;
		}
	}
}
