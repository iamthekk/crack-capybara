using System;

namespace LocalModels.Bean
{
	public class Shop_Shop : BaseLocalBean
	{
		public int id { get; set; }

		public int shopType { get; set; }

		public string[] cost { get; set; }

		public int adId { get; set; }

		public string[] products { get; set; }

		public string nameId { get; set; }

		public string descId { get; set; }

		public int iconAtlasId { get; set; }

		public string iconName { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.shopType = base.readInt();
			this.cost = base.readArraystring();
			this.adId = base.readInt();
			this.products = base.readArraystring();
			this.nameId = base.readLocalString();
			this.descId = base.readLocalString();
			this.iconAtlasId = base.readInt();
			this.iconName = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Shop_Shop();
		}
	}
}
