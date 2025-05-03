using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ArtSagecraft_SagecraftModel : BaseLocalModel
	{
		public ArtSagecraft_Sagecraft GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ArtSagecraft_Sagecraft> GetAllElements()
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

		public static readonly string fileName = "ArtSagecraft_Sagecraft";

		private ArtSagecraft_SagecraftModelImpl modelImpl = new ArtSagecraft_SagecraftModelImpl();
	}
}
