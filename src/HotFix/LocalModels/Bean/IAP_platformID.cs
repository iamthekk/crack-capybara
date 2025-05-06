using System;

namespace LocalModels.Bean
{
	public class IAP_platformID : BaseLocalBean
	{
		public int id { get; set; }

		public string productID { get; set; }

		public string CNproductID { get; set; }

		public float price { get; set; }

		public float CNprice { get; set; }

		public float CNpriceCent { get; set; }

		public int productType { get; set; }

		public string CNproductTitle { get; set; }

		public string productTitleID { get; set; }

		public string productDescID { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.productID = base.readLocalString();
			this.CNproductID = base.readLocalString();
			this.price = base.readFloat();
			this.CNprice = base.readFloat();
			this.CNpriceCent = base.readFloat();
			this.productType = base.readInt();
			this.CNproductTitle = base.readLocalString();
			this.productTitleID = base.readLocalString();
			this.productDescID = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new IAP_platformID();
		}
	}
}
