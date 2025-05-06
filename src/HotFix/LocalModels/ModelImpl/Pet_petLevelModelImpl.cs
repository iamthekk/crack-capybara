using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Pet_petLevelModelImpl : BaseLocalModelImpl<Pet_petLevel, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Pet_petLevel();
		}

		protected override int GetBeanKey(Pet_petLevel bean)
		{
			return bean.id;
		}
	}
}
