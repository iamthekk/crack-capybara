using System;

namespace LocalModels.Bean
{
	public class WorldBoss_RankReward : BaseLocalBean
	{
		public int ID { get; set; }

		public int Tag { get; set; }

		public int SubsectionID { get; set; }

		public int[] RankRange { get; set; }

		public string[] RoundReward { get; set; }

		public string[] SeasonReward { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.Tag = base.readInt();
			this.SubsectionID = base.readInt();
			this.RankRange = base.readArrayint();
			this.RoundReward = base.readArraystring();
			this.SeasonReward = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new WorldBoss_RankReward();
		}
	}
}
