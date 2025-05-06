using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Shop_SummonModelImpl : BaseLocalModelImpl<Shop_Summon, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Shop_Summon();
		}

		protected override int GetBeanKey(Shop_Summon bean)
		{
			return bean.id;
		}
	}
}
