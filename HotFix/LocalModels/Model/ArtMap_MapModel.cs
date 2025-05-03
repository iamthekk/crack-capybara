using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ArtMap_MapModel : BaseLocalModel
	{
		public ArtMap_Map GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ArtMap_Map> GetAllElements()
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

		public static readonly string fileName = "ArtMap_Map";

		private ArtMap_MapModelImpl modelImpl = new ArtMap_MapModelImpl();
	}
}
