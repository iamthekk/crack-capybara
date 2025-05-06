using System;

namespace LocalModels.Bean
{
	public class Shop_ShopSell : BaseLocalBean
	{
		public int id { get; set; }

		public int type { get; set; }

		public int orderId { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.type = base.readInt();
			this.orderId = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Shop_ShopSell();
		}
	}
}
