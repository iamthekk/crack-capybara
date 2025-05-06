using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Dungeon_DungeonBaseModel : BaseLocalModel
	{
		public Dungeon_DungeonBase GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Dungeon_DungeonBase> GetAllElements()
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

		public static readonly string fileName = "Dungeon_DungeonBase";

		private Dungeon_DungeonBaseModelImpl modelImpl = new Dungeon_DungeonBaseModelImpl();
	}
}
