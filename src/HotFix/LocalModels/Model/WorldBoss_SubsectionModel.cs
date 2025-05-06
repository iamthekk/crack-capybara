using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class WorldBoss_SubsectionModel : BaseLocalModel
	{
		public WorldBoss_Subsection GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<WorldBoss_Subsection> GetAllElements()
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

		public static readonly string fileName = "WorldBoss_Subsection";

		private WorldBoss_SubsectionModelImpl modelImpl = new WorldBoss_SubsectionModelImpl();
	}
}
