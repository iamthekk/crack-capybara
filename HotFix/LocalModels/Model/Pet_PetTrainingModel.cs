using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Pet_PetTrainingModel : BaseLocalModel
	{
		public Pet_PetTraining GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Pet_PetTraining> GetAllElements()
		{
			return this.modelImpl.GetAllElement();
		}

		public override void Initialise(string name, byte[] assetBytes)
		{
			base.Initialise(name, assetBytes);
			if (assetBytes == null)
			{
				return;
			}
			this.modelImpl.Initialise(name, assetBytes);
		}

		public override void DeInitialise()
		{
			this.modelImpl.DeInitialise();
			base.DeInitialise();
		}

		public static readonly string fileName = "Pet_PetTraining";

		private Pet_PetTrainingModelImpl modelImpl = new Pet_PetTrainingModelImpl();
	}
}
