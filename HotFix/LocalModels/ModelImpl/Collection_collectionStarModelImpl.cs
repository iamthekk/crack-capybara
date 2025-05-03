using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Collection_collectionStarModelImpl : BaseLocalModelImpl<Collection_collectionStar, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Collection_collectionStar();
		}

		protected override int GetBeanKey(Collection_collectionStar bean)
		{
			return bean.id;
		}
	}
}
