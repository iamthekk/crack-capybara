using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ArtHover_hoverTextModel : BaseLocalModel
	{
		public ArtHover_hoverText GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ArtHover_hoverText> GetAllElements()
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

		public static readonly string fileName = "ArtHover_hoverText";

		private ArtHover_hoverTextModelImpl modelImpl = new ArtHover_hoverTextModelImpl();
	}
}
