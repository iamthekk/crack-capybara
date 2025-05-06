using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Pet_petSummonModelImpl : BaseLocalModelImpl<Pet_petSummon, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Pet_petSummon();
		}

		protected override int GetBeanKey(Pet_petSummon bean)
		{
			return bean.id;
		}
	}
}
