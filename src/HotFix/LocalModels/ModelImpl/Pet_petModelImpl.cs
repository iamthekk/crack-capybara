using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Pet_petModelImpl : BaseLocalModelImpl<Pet_pet, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Pet_pet();
		}

		protected override int GetBeanKey(Pet_pet bean)
		{
			return bean.id;
		}
	}
}
