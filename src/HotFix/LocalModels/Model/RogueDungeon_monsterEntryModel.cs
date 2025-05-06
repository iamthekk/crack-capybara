using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class RogueDungeon_monsterEntryModel : BaseLocalModel
	{
		public RogueDungeon_monsterEntry GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<RogueDungeon_monsterEntry> GetAllElements()
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

		public static readonly string fileName = "RogueDungeon_monsterEntry";

		private RogueDungeon_monsterEntryModelImpl modelImpl = new RogueDungeon_monsterEntryModelImpl();
	}
}
