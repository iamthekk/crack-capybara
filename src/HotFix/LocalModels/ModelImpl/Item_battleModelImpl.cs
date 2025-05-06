using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Item_battleModelImpl : BaseLocalModelImpl<Item_battle, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Item_battle();
		}

		protected override int GetBeanKey(Item_battle bean)
		{
			return bean.id;
		}
	}
}
