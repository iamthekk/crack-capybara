using System;

namespace LocalModels.Bean
{
	public class Shop_Summon : BaseLocalBean
	{
		public int id { get; set; }

		public int orderId { get; set; }

		public int groupId { get; set; }

		public int freeTimes { get; set; }

		public int adId { get; set; }

		public int priceId { get; set; }

		public int singlePrice { get; set; }

		public int singlePriceOrigin { get; set; }

		public int quickDraw { get; set; }

		public int tenPrice { get; set; }

		public int tenPriceOrigin { get; set; }

		public string[] first { get; set; }

		public string[] rateShow { get; set; }

		public string[] normalRate { get; set; }

		public string[] reverseRate { get; set; }

		public int reverseCount { get; set; }

		public int reversePool { get; set; }

		public int miniPityCount { get; set; }

		public string[] miniPityRate { get; set; }

		public int miniPityPool { get; set; }

		public int hardPityCount { get; set; }

		public string[] hardPityRate { get; set; }

		public int hardPityPool { get; set; }

		public int upEquipID { get; set; }

		public int boxId { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.orderId = base.readInt();
			this.groupId = base.readInt();
			this.freeTimes = base.readInt();
			this.adId = base.readInt();
			this.priceId = base.readInt();
			this.singlePrice = base.readInt();
			this.singlePriceOrigin = base.readInt();
			this.quickDraw = base.readInt();
			this.tenPrice = base.readInt();
			this.tenPriceOrigin = base.readInt();
			this.first = base.readArraystring();
			this.rateShow = base.readArraystring();
			this.normalRate = base.readArraystring();
			this.reverseRate = base.readArraystring();
			this.reverseCount = base.readInt();
			this.reversePool = base.readInt();
			this.miniPityCount = base.readInt();
			this.miniPityRate = base.readArraystring();
			this.miniPityPool = base.readInt();
			this.hardPityCount = base.readInt();
			this.hardPityRate = base.readArraystring();
			this.hardPityPool = base.readInt();
			this.upEquipID = base.readInt();
			this.boxId = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Shop_Summon();
		}
	}
}
