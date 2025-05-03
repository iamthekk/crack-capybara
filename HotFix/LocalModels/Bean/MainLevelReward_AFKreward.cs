using System;

namespace LocalModels.Bean
{
	public class MainLevelReward_AFKreward : BaseLocalBean
	{
		public int ID { get; set; }

		public int RewardLevel { get; set; }

		public int RequiredLevel { get; set; }

		public int HangGold { get; set; }

		public int HangDust { get; set; }

		public int HangHeroExp { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.RewardLevel = base.readInt();
			this.RequiredLevel = base.readInt();
			this.HangGold = base.readInt();
			this.HangDust = base.readInt();
			this.HangHeroExp = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new MainLevelReward_AFKreward();
		}
	}
}
