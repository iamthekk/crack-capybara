using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Pet_petCollectionModel : BaseLocalModel
	{
		public Pet_petCollection GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Pet_petCollection> GetAllElements()
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

		public static readonly string fileName = "Pet_petCollection";

		private Pet_petCollectionModelImpl modelImpl = new Pet_petCollectionModelImpl();
	}
}
