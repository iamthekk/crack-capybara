using System;

namespace LocalModels.Bean
{
	public class Guild_guildGiftLevel : BaseLocalBean
	{
		public int ID { get; set; }

		public int NeedExp { get; set; }

		public int NeedKey { get; set; }

		public string[] Reward { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.NeedExp = base.readInt();
			this.NeedKey = base.readInt();
			this.Reward = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Guild_guildGiftLevel();
		}
	}
}
