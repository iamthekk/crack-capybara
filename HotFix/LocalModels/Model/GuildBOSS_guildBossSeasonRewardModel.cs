using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class GuildBOSS_guildBossSeasonRewardModel : BaseLocalModel
	{
		public GuildBOSS_guildBossSeasonReward GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<GuildBOSS_guildBossSeasonReward> GetAllElements()
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

		public static readonly string fileName = "GuildBOSS_guildBossSeasonReward";

		private GuildBOSS_guildBossSeasonRewardModelImpl modelImpl = new GuildBOSS_guildBossSeasonRewardModelImpl();
	}
}
