using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Pet_petSummonModel : BaseLocalModel
	{
		public Pet_petSummon GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Pet_petSummon> GetAllElements()
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

		public static readonly string fileName = "Pet_petSummon";

		private Pet_petSummonModelImpl modelImpl = new Pet_petSummonModelImpl();
	}
}
