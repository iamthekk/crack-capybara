using System;

namespace LocalModels.Bean
{
	public class ChestList_ChestReward : BaseLocalBean
	{
		public int id { get; set; }

		public int chestType { get; set; }

		public int itemId { get; set; }

		public int openPoint { get; set; }

		public int coreReward { get; set; }

		public int rewardTimes { get; set; }

		public int reward { get; set; }

		public string name { get; set; }

		public string desc { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.chestType = base.readInt();
			this.itemId = base.readInt();
			this.openPoint = base.readInt();
			this.coreReward = base.readInt();
			this.rewardTimes = base.readInt();
			this.reward = base.readInt();
			this.name = base.readLocalString();
			this.desc = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ChestList_ChestReward();
		}
	}
}
