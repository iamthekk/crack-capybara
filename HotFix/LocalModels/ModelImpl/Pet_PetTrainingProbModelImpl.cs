using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Pet_PetTrainingProbModelImpl : BaseLocalModelImpl<Pet_PetTrainingProb, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Pet_PetTrainingProb();
		}

		protected override int GetBeanKey(Pet_PetTrainingProb bean)
		{
			return bean.id;
		}
	}
}
