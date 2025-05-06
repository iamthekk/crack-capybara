using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Pet_petLevelEffectModel : BaseLocalModel
	{
		public Pet_petLevelEffect GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Pet_petLevelEffect> GetAllElements()
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

		public static readonly string fileName = "Pet_petLevelEffect";

		private Pet_petLevelEffectModelImpl modelImpl = new Pet_petLevelEffectModelImpl();
	}
}
