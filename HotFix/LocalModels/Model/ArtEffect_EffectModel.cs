using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ArtEffect_EffectModel : BaseLocalModel
	{
		public ArtEffect_Effect GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ArtEffect_Effect> GetAllElements()
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

		public static readonly string fileName = "ArtEffect_Effect";

		private ArtEffect_EffectModelImpl modelImpl = new ArtEffect_EffectModelImpl();
	}
}
