using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Collection_collectionModelImpl : BaseLocalModelImpl<Collection_collection, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Collection_collection();
		}

		protected override int GetBeanKey(Collection_collection bean)
		{
			return bean.id;
		}
	}
}
