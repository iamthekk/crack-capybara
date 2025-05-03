using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Mining_miningBaseModel : BaseLocalModel
	{
		public Mining_miningBase GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Mining_miningBase> GetAllElements()
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

		public static readonly string fileName = "Mining_miningBase";

		private Mining_miningBaseModelImpl modelImpl = new Mining_miningBaseModelImpl();
	}
}
