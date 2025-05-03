using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ItemResources_itemgetModelImpl : BaseLocalModelImpl<ItemResources_itemget, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ItemResources_itemget();
		}

		protected override int GetBeanKey(ItemResources_itemget bean)
		{
			return bean.id;
		}
	}
}
