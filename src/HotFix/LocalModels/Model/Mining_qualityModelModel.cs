using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Mining_qualityModelModel : BaseLocalModel
	{
		public Mining_qualityModel GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Mining_qualityModel> GetAllElements()
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

		public static readonly string fileName = "Mining_qualityModel";

		private Mining_qualityModelModelImpl modelImpl = new Mining_qualityModelModelImpl();
	}
}
