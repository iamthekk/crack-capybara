using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ItemResources_jumpResourceModelImpl : BaseLocalModelImpl<ItemResources_jumpResource, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ItemResources_jumpResource();
		}

		protected override int GetBeanKey(ItemResources_jumpResource bean)
		{
			return bean.id;
		}
	}
}
