using System;

namespace LocalModels.Bean
{
	public class Shop_ShopActivity : BaseLocalBean
	{
		public int id { get; set; }

		public int type { get; set; }

		public int orderId { get; set; }

		public int linkType { get; set; }

		public int linkId { get; set; }

		public int openType { get; set; }

		public string startTime { get; set; }

		public string endTime { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.type = base.readInt();
			this.orderId = base.readInt();
			this.linkType = base.readInt();
			this.linkId = base.readInt();
			this.openType = base.readInt();
			this.startTime = base.readLocalString();
			this.endTime = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Shop_ShopActivity();
		}
	}
}
