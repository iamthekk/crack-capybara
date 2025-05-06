using System;

namespace LocalModels.Bean
{
	public class IAP_GiftPacks : BaseLocalBean
	{
		public int id { get; set; }

		public int packType { get; set; }

		public string[] products { get; set; }

		public int group { get; set; }

		public int hide { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.packType = base.readInt();
			this.products = base.readArraystring();
			this.group = base.readInt();
			this.hide = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new IAP_GiftPacks();
		}
	}
}
