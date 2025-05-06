using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Shop_EquipActivityModelImpl : BaseLocalModelImpl<Shop_EquipActivity, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Shop_EquipActivity();
		}

		protected override int GetBeanKey(Shop_EquipActivity bean)
		{
			return bean.id;
		}
	}
}
