using System;

namespace LocalModels.Bean
{
	public class TicketExchange_Exchange : BaseLocalBean
	{
		public int id { get; set; }

		public int type { get; set; }

		public int count { get; set; }

		public int unit { get; set; }

		public int diamondCost { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.type = base.readInt();
			this.count = base.readInt();
			this.unit = base.readInt();
			this.diamondCost = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new TicketExchange_Exchange();
		}
	}
}
