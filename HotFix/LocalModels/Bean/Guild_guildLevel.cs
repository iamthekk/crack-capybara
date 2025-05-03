using System;

namespace LocalModels.Bean
{
	public class Guild_guildLevel : BaseLocalBean
	{
		public int ID { get; set; }

		public int Exp { get; set; }

		public int MaxMemberCount { get; set; }

		public int MaxContribute { get; set; }

		public int[] MaxPositionCount { get; set; }

		public int[] ShopItemCount { get; set; }

		public int TaskCount { get; set; }

		public int GuildBossOpen { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.Exp = base.readInt();
			this.MaxMemberCount = base.readInt();
			this.MaxContribute = base.readInt();
			this.MaxPositionCount = base.readArrayint();
			this.ShopItemCount = base.readArrayint();
			this.TaskCount = base.readInt();
			this.GuildBossOpen = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Guild_guildLevel();
		}
	}
}
