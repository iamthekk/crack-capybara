using System;

namespace LocalModels.Bean
{
	public class GuildBOSS_guildBossSeasonReward : BaseLocalBean
	{
		public int ID { get; set; }

		public int levelID { get; set; }

		public string[] ranking { get; set; }

		public string[] weeklyRewards { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.levelID = base.readInt();
			this.ranking = base.readArraystring();
			this.weeklyRewards = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GuildBOSS_guildBossSeasonReward();
		}
	}
}
