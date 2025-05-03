using System;

namespace LocalModels.Bean
{
	public class IAP_DiamondPacks : BaseLocalBean
	{
		public int id { get; set; }

		public string[] products { get; set; }

		public string parameters { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.products = base.readArraystring();
			this.parameters = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new IAP_DiamondPacks();
		}
	}
}
