using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Dungeon_DungeonLevelModel : BaseLocalModel
	{
		public Dungeon_DungeonLevel GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Dungeon_DungeonLevel> GetAllElements()
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

		public static readonly string fileName = "Dungeon_DungeonLevel";

		private Dungeon_DungeonLevelModelImpl modelImpl = new Dungeon_DungeonLevelModelImpl();
	}
}
