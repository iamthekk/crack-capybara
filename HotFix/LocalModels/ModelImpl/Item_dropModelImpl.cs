using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Item_dropModelImpl : BaseLocalModelImpl<Item_drop, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Item_drop();
		}

		protected override int GetBeanKey(Item_drop bean)
		{
			return bean.drop_id;
		}
	}
}
