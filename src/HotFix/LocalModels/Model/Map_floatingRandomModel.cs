using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Map_floatingRandomModel : BaseLocalModel
	{
		public Map_floatingRandom GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Map_floatingRandom> GetAllElements()
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

		public static readonly string fileName = "Map_floatingRandom";

		private Map_floatingRandomModelImpl modelImpl = new Map_floatingRandomModelImpl();
	}
}
