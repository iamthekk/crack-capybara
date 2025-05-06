using System;

namespace LocalModels.Bean
{
	public class Guild_guildDonation : BaseLocalBean
	{
		public int ID { get; set; }

		public int Count { get; set; }

		public int[] GuildLevel { get; set; }

		public int[] ChapterId { get; set; }

		public string[] Reward { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.Count = base.readInt();
			this.GuildLevel = base.readArrayint();
			this.ChapterId = base.readArrayint();
			this.Reward = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Guild_guildDonation();
		}
	}
}
