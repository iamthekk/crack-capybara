using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Quality_collectionQualityModel : BaseLocalModel
	{
		public Quality_collectionQuality GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Quality_collectionQuality> GetAllElements()
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

		public static readonly string fileName = "Quality_collectionQuality";

		private Quality_collectionQualityModelImpl modelImpl = new Quality_collectionQualityModelImpl();
	}
}
