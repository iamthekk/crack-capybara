using System;

namespace LocalModels.Bean
{
	public class IAP_ChapterPacks : BaseLocalBean
	{
		public int id { get; set; }

		public int chapterId { get; set; }

		public int orderId { get; set; }

		public string[] products { get; set; }

		public string nameId { get; set; }

		public string descId { get; set; }

		public string descColor { get; set; }

		public string originPriceColor { get; set; }

		public int bannerBgType { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.chapterId = base.readInt();
			this.orderId = base.readInt();
			this.products = base.readArraystring();
			this.nameId = base.readLocalString();
			this.descId = base.readLocalString();
			this.descColor = base.readLocalString();
			this.originPriceColor = base.readLocalString();
			this.bannerBgType = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new IAP_ChapterPacks();
		}
	}
}
