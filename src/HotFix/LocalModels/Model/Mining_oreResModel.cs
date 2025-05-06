using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Mining_oreResModel : BaseLocalModel
	{
		public Mining_oreRes GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Mining_oreRes> GetAllElements()
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

		public static readonly string fileName = "Mining_oreRes";

		private Mining_oreResModelImpl modelImpl = new Mining_oreResModelImpl();
	}
}
