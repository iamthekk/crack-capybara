using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ItemGift_ItemGiftModelImpl : BaseLocalModelImpl<ItemGift_ItemGift, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ItemGift_ItemGift();
		}

		protected override int GetBeanKey(ItemGift_ItemGift bean)
		{
			return bean.id;
		}
	}
}
