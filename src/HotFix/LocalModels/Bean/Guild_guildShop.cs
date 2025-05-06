using System;

namespace LocalModels.Bean
{
	public class Guild_guildShop : BaseLocalBean
	{
		public int ID { get; set; }

		public int GuildLevel { get; set; }

		public int Type { get; set; }

		public int[] Condition { get; set; }

		public int[] Price { get; set; }

		public string[] Reward { get; set; }

		public int Limit { get; set; }

		public int FreeCnt { get; set; }

		public int Weight { get; set; }

		public int Position { get; set; }

		public string[] Discount { get; set; }

		public int bgColor { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.GuildLevel = base.readInt();
			this.Type = base.readInt();
			this.Condition = base.readArrayint();
			this.Price = base.readArrayint();
			this.Reward = base.readArraystring();
			this.Limit = base.readInt();
			this.FreeCnt = base.readInt();
			this.Weight = base.readInt();
			this.Position = base.readInt();
			this.Discount = base.readArraystring();
			this.bgColor = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Guild_guildShop();
		}
	}
}
