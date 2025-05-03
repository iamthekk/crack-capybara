using System;

namespace LocalModels.Bean
{
	public class SevenDay_SevenDayActiveReward : BaseLocalBean
	{
		public int ID { get; set; }

		public int NeedActive { get; set; }

		public string[] Reward { get; set; }

		public int IfEquip { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.NeedActive = base.readInt();
			this.Reward = base.readArraystring();
			this.IfEquip = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new SevenDay_SevenDayActiveReward();
		}
	}
}
