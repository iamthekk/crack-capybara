using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class WorldBoss_WorldBossModel : BaseLocalModel
	{
		public WorldBoss_WorldBoss GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<WorldBoss_WorldBoss> GetAllElements()
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

		public static readonly string fileName = "WorldBoss_WorldBoss";

		private WorldBoss_WorldBossModelImpl modelImpl = new WorldBoss_WorldBossModelImpl();
	}
}
