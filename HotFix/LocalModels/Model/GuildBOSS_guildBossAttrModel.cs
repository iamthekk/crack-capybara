using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class GuildBOSS_guildBossAttrModel : BaseLocalModel
	{
		public GuildBOSS_guildBossAttr GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<GuildBOSS_guildBossAttr> GetAllElements()
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

		public static readonly string fileName = "GuildBOSS_guildBossAttr";

		private GuildBOSS_guildBossAttrModelImpl modelImpl = new GuildBOSS_guildBossAttrModelImpl();
	}
}
