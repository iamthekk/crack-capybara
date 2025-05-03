using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class GuildBOSS_guildBossStepModel : BaseLocalModel
	{
		public GuildBOSS_guildBossStep GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<GuildBOSS_guildBossStep> GetAllElements()
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

		public static readonly string fileName = "GuildBOSS_guildBossStep";

		private GuildBOSS_guildBossStepModelImpl modelImpl = new GuildBOSS_guildBossStepModelImpl();
	}
}
