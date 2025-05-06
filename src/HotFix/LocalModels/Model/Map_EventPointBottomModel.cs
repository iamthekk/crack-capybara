using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Map_EventPointBottomModel : BaseLocalModel
	{
		public Map_EventPointBottom GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Map_EventPointBottom> GetAllElements()
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

		public static readonly string fileName = "Map_EventPointBottom";

		private Map_EventPointBottomModelImpl modelImpl = new Map_EventPointBottomModelImpl();
	}
}
