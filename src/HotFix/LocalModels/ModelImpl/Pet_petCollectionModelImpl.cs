using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Pet_petCollectionModelImpl : BaseLocalModelImpl<Pet_petCollection, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Pet_petCollection();
		}

		protected override int GetBeanKey(Pet_petCollection bean)
		{
			return bean.id;
		}
	}
}
