using System;

namespace LocalModels.Bean
{
	public class GuildBOSS_rankRewards : BaseLocalBean
	{
		public int ID { get; set; }

		public int BossId { get; set; }

		public int[] RankRange { get; set; }

		public string[] Reward { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.BossId = base.readInt();
			this.RankRange = base.readArrayint();
			this.Reward = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GuildBOSS_rankRewards();
		}
	}
}
