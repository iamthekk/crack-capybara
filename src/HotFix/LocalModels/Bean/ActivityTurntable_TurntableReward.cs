using System;

namespace LocalModels.Bean
{
	public class ActivityTurntable_TurntableReward : BaseLocalBean
	{
		public int id { get; set; }

		public int rewardID { get; set; }

		public int point { get; set; }

		public string[] reward { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.rewardID = base.readInt();
			this.point = base.readInt();
			this.reward = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ActivityTurntable_TurntableReward();
		}
	}
}
