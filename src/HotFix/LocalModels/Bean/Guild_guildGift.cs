using System;

namespace LocalModels.Bean
{
	public class Guild_guildGift : BaseLocalBean
	{
		public int ID { get; set; }

		public int Type { get; set; }

		public string Icon { get; set; }

		public string Name { get; set; }

		public int Reward { get; set; }

		public int GetKey { get; set; }

		public int GetExp { get; set; }

		public int DurationTimes { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.Type = base.readInt();
			this.Icon = base.readLocalString();
			this.Name = base.readLocalString();
			this.Reward = base.readInt();
			this.GetKey = base.readInt();
			this.GetExp = base.readInt();
			this.DurationTimes = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Guild_guildGift();
		}
	}
}
