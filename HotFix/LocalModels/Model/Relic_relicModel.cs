﻿using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Relic_relicModel : BaseLocalModel
	{
		public Relic_relic GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Relic_relic> GetAllElements()
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

		public static readonly string fileName = "Relic_relic";

		private Relic_relicModelImpl modelImpl = new Relic_relicModelImpl();
	}
}
