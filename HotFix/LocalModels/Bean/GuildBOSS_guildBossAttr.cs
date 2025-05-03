using System;

namespace LocalModels.Bean
{
	public class GuildBOSS_guildBossAttr : BaseLocalBean
	{
		public int id { get; set; }

		public string attributes { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.attributes = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GuildBOSS_guildBossAttr();
		}
	}
}
