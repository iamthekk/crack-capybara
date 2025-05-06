using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ArtSkin_equipSkinModel : BaseLocalModel
	{
		public ArtSkin_equipSkin GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ArtSkin_equipSkin> GetAllElements()
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

		public static readonly string fileName = "ArtSkin_equipSkin";

		private ArtSkin_equipSkinModelImpl modelImpl = new ArtSkin_equipSkinModelImpl();
	}
}
