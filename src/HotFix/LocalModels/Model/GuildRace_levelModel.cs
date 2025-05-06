using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class GuildRace_levelModel : BaseLocalModel
	{
		public GuildRace_level GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<GuildRace_level> GetAllElements()
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

		public static readonly string fileName = "GuildRace_level";

		private GuildRace_levelModelImpl modelImpl = new GuildRace_levelModelImpl();
	}
}
