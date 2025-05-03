using System;

namespace LocalModels.Bean
{
	public class GuildBOSS_guildBossMonster : BaseLocalBean
	{
		public int id { get; set; }

		public string pos1 { get; set; }

		public string pos2 { get; set; }

		public string pos3 { get; set; }

		public string pos4 { get; set; }

		public string pos5 { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.pos1 = base.readLocalString();
			this.pos2 = base.readLocalString();
			this.pos3 = base.readLocalString();
			this.pos4 = base.readLocalString();
			this.pos5 = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GuildBOSS_guildBossMonster();
		}
	}
}
