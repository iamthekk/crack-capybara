using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class GuildRace_opentimeModel : BaseLocalModel
	{
		public GuildRace_opentime GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<GuildRace_opentime> GetAllElements()
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

		public static readonly string fileName = "GuildRace_opentime";

		private GuildRace_opentimeModelImpl modelImpl = new GuildRace_opentimeModelImpl();
	}
}
