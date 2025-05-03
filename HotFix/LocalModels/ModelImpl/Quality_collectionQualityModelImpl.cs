using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Quality_collectionQualityModelImpl : BaseLocalModelImpl<Quality_collectionQuality, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Quality_collectionQuality();
		}

		protected override int GetBeanKey(Quality_collectionQuality bean)
		{
			return bean.id;
		}
	}
}
