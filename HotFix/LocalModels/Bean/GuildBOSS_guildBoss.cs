using System;

namespace LocalModels.Bean
{
	public class GuildBOSS_guildBoss : BaseLocalBean
	{
		public int ID { get; set; }

		public int BossId { get; set; }

		public string NameID { get; set; }

		public float uiScale { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.BossId = base.readInt();
			this.NameID = base.readLocalString();
			this.uiScale = base.readFloat();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GuildBOSS_guildBoss();
		}
	}
}
