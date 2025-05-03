using System;

namespace LocalModels.Bean
{
	public class GuildBOSS_guildBossTask : BaseLocalBean
	{
		public int ID { get; set; }

		public int Type { get; set; }

		public long Need { get; set; }

		public string[] Reward { get; set; }

		public string[] OtherReward { get; set; }

		public string languageId { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.Type = base.readInt();
			this.Need = base.readLong();
			this.Reward = base.readArraystring();
			this.OtherReward = base.readArraystring();
			this.languageId = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GuildBOSS_guildBossTask();
		}
	}
}
