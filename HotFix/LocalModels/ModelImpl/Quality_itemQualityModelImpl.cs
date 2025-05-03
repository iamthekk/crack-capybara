using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Quality_itemQualityModelImpl : BaseLocalModelImpl<Quality_itemQuality, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Quality_itemQuality();
		}

		protected override int GetBeanKey(Quality_itemQuality bean)
		{
			return bean.id;
		}
	}
}
