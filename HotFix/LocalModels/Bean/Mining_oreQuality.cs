using System;

namespace LocalModels.Bean
{
	public class Mining_oreQuality : BaseLocalBean
	{
		public int id { get; set; }

		public int upgradeRate { get; set; }

		public string languageId { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.upgradeRate = base.readInt();
			this.languageId = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Mining_oreQuality();
		}
	}
}
