using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Item_dropLvModelImpl : BaseLocalModelImpl<Item_dropLv, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Item_dropLv();
		}

		protected override int GetBeanKey(Item_dropLv bean)
		{
			return bean.drop_id;
		}
	}
}
