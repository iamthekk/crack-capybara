using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Quality_equipQualityModel : BaseLocalModel
	{
		public Quality_equipQuality GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Quality_equipQuality> GetAllElements()
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

		public static readonly string fileName = "Quality_equipQuality";

		private Quality_equipQualityModelImpl modelImpl = new Quality_equipQualityModelImpl();
	}
}
