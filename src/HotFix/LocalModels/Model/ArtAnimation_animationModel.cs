using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ArtAnimation_animationModel : BaseLocalModel
	{
		public ArtAnimation_animation GetElementById(string id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ArtAnimation_animation> GetAllElements()
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

		public static readonly string fileName = "ArtAnimation_animation";

		private ArtAnimation_animationModelImpl modelImpl = new ArtAnimation_animationModelImpl();
	}
}
