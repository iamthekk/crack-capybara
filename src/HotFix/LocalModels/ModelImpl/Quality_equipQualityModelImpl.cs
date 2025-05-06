using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Quality_equipQualityModelImpl : BaseLocalModelImpl<Quality_equipQuality, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Quality_equipQuality();
		}

		protected override int GetBeanKey(Quality_equipQuality bean)
		{
			return bean.id;
		}
	}
}
