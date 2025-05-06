using System;

namespace LocalModels.Bean
{
	public class IAP_Purchase : BaseLocalBean
	{
		public int id { get; set; }

		public string notes { get; set; }

		public string titleID { get; set; }

		public string nameID { get; set; }

		public string descID { get; set; }

		public int iconAtlasID { get; set; }

		public string iconName { get; set; }

		public int productType { get; set; }

		public int source { get; set; }

		public int viewType { get; set; }

		public int platformID { get; set; }

		public float price1 { get; set; }

		public float originPrice1 { get; set; }

		public float CNprice { get; set; }

		public float CNoriginPrice { get; set; }

		public int limitCount { get; set; }

		public int priority { get; set; }

		public string[] showProducts { get; set; }

		public int valueBet { get; set; }

		public int guildGift { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.notes = base.readLocalString();
			this.titleID = base.readLocalString();
			this.nameID = base.readLocalString();
			this.descID = base.readLocalString();
			this.iconAtlasID = base.readInt();
			this.iconName = base.readLocalString();
			this.productType = base.readInt();
			this.source = base.readInt();
			this.viewType = base.readInt();
			this.platformID = base.readInt();
			this.price1 = base.readFloat();
			this.originPrice1 = base.readFloat();
			this.CNprice = base.readFloat();
			this.CNoriginPrice = base.readFloat();
			this.limitCount = base.readInt();
			this.priority = base.readInt();
			this.showProducts = base.readArraystring();
			this.valueBet = base.readInt();
			this.guildGift = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new IAP_Purchase();
		}
	}
}
