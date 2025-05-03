using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Pet_PetTrainingModelImpl : BaseLocalModelImpl<Pet_PetTraining, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Pet_PetTraining();
		}

		protected override int GetBeanKey(Pet_PetTraining bean)
		{
			return bean.id;
		}
	}
}
