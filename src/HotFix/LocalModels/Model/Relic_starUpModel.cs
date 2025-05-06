using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Relic_starUpModel : BaseLocalModel
	{
		public Relic_starUp GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Relic_starUp> GetAllElements()
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

		public static readonly string fileName = "Relic_starUp";

		private Relic_starUpModelImpl modelImpl = new Relic_starUpModelImpl();
	}
}
