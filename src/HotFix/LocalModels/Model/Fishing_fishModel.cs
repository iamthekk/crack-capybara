using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Fishing_fishModel : BaseLocalModel
	{
		public Fishing_fish GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Fishing_fish> GetAllElements()
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

		public static readonly string fileName = "Fishing_fish";

		private Fishing_fishModelImpl modelImpl = new Fishing_fishModelImpl();
	}
}
