using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Pet_PetEntryModelImpl : BaseLocalModelImpl<Pet_PetEntry, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Pet_PetEntry();
		}

		protected override int GetBeanKey(Pet_PetEntry bean)
		{
			return bean.id;
		}
	}
}
