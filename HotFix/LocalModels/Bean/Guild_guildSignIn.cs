using System;

namespace LocalModels.Bean
{
	public class Guild_guildSignIn : BaseLocalBean
	{
		public int ID { get; set; }

		public int NeedItemId { get; set; }

		public int NeedItemCount { get; set; }

		public string[] Reward { get; set; }

		public string[] OtherReward { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.NeedItemId = base.readInt();
			this.NeedItemCount = base.readInt();
			this.Reward = base.readArraystring();
			this.OtherReward = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Guild_guildSignIn();
		}
	}
}
