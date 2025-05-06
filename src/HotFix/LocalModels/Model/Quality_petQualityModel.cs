﻿using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Quality_petQualityModel : BaseLocalModel
	{
		public Quality_petQuality GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Quality_petQuality> GetAllElements()
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

		public static readonly string fileName = "Quality_petQuality";

		private Quality_petQualityModelImpl modelImpl = new Quality_petQualityModelImpl();
	}
}
