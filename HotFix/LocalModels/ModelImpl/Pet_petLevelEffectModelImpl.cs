using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Pet_petLevelEffectModelImpl : BaseLocalModelImpl<Pet_petLevelEffect, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Pet_petLevelEffect();
		}

		protected override int GetBeanKey(Pet_petLevelEffect bean)
		{
			return bean.id;
		}
	}
}
