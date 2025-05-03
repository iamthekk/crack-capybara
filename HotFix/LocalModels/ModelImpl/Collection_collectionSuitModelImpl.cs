using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Collection_collectionSuitModelImpl : BaseLocalModelImpl<Collection_collectionSuit, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Collection_collectionSuit();
		}

		protected override int GetBeanKey(Collection_collectionSuit bean)
		{
			return bean.id;
		}
	}
}
