using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Quality_itemQualityModel : BaseLocalModel
	{
		public Quality_itemQuality GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Quality_itemQuality> GetAllElements()
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

		public static readonly string fileName = "Quality_itemQuality";

		private Quality_itemQualityModelImpl modelImpl = new Quality_itemQualityModelImpl();
	}
}
