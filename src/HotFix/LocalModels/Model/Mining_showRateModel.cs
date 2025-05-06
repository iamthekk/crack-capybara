using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Mining_showRateModel : BaseLocalModel
	{
		public Mining_showRate GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Mining_showRate> GetAllElements()
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

		public static readonly string fileName = "Mining_showRate";

		private Mining_showRateModelImpl modelImpl = new Mining_showRateModelImpl();
	}
}
