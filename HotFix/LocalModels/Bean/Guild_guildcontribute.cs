using System;

namespace LocalModels.Bean
{
	public class Guild_guildcontribute : BaseLocalBean
	{
		public int Times { get; set; }

		public string[] CostItem { get; set; }

		public string[] guildItems { get; set; }

		public override bool readImpl()
		{
			this.Times = base.readInt();
			this.CostItem = base.readArraystring();
			this.guildItems = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Guild_guildcontribute();
		}
	}
}
