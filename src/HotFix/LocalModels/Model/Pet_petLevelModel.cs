using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Pet_petLevelModel : BaseLocalModel
	{
		public Pet_petLevel GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Pet_petLevel> GetAllElements()
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

		public static readonly string fileName = "Pet_petLevel";

		private Pet_petLevelModelImpl modelImpl = new Pet_petLevelModelImpl();
	}
}
