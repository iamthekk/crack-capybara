using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ArtHit_HitModel : BaseLocalModel
	{
		public ArtHit_Hit GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ArtHit_Hit> GetAllElements()
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

		public static readonly string fileName = "ArtHit_Hit";

		private ArtHit_HitModelImpl modelImpl = new ArtHit_HitModelImpl();
	}
}
