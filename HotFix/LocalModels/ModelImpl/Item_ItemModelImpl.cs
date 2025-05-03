using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Item_ItemModelImpl : BaseLocalModelImpl<Item_Item, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Item_Item();
		}

		protected override int GetBeanKey(Item_Item bean)
		{
			return bean.id;
		}
	}
}
