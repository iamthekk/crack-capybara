using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class GuildBOSS_guildBossMonsterModel : BaseLocalModel
	{
		public GuildBOSS_guildBossMonster GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<GuildBOSS_guildBossMonster> GetAllElements()
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

		public static readonly string fileName = "GuildBOSS_guildBossMonster";

		private GuildBOSS_guildBossMonsterModelImpl modelImpl = new GuildBOSS_guildBossMonsterModelImpl();
	}
}
