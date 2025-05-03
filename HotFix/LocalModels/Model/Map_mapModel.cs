using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Map_mapModel : BaseLocalModel
	{
		public Map_map GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Map_map> GetAllElements()
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

		public static readonly string fileName = "Map_map";

		private Map_mapModelImpl modelImpl = new Map_mapModelImpl();
	}
}
