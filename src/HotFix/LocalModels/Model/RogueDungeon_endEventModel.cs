using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class RogueDungeon_endEventModel : BaseLocalModel
	{
		public RogueDungeon_endEvent GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<RogueDungeon_endEvent> GetAllElements()
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

		public static readonly string fileName = "RogueDungeon_endEvent";

		private RogueDungeon_endEventModelImpl modelImpl = new RogueDungeon_endEventModelImpl();
	}
}
