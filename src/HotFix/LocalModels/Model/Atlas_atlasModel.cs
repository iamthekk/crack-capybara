using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Atlas_atlasModel : BaseLocalModel
	{
		public Atlas_atlas GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Atlas_atlas> GetAllElements()
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

		public static readonly string fileName = "Atlas_atlas";

		private Atlas_atlasModelImpl modelImpl = new Atlas_atlasModelImpl();
	}
}
