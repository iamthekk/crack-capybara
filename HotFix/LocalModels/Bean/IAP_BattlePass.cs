using System;

namespace LocalModels.Bean
{
	public class IAP_BattlePass : BaseLocalBean
	{
		public int id { get; set; }

		public string notes { get; set; }

		public int seasonID { get; set; }

		public string nameID { get; set; }

		public int groupId { get; set; }

		public int buyTime { get; set; }

		public int startTime { get; set; }

		public int endTime { get; set; }

		public int finalRewardTimes { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.notes = base.readLocalString();
			this.seasonID = base.readInt();
			this.nameID = base.readLocalString();
			this.groupId = base.readInt();
			this.buyTime = base.readInt();
			this.startTime = base.readInt();
			this.endTime = base.readInt();
			this.finalRewardTimes = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new IAP_BattlePass();
		}
	}
}
