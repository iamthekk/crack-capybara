using System;

namespace LocalModels.Bean
{
	public class Shop_Ad : BaseLocalBean
	{
		public int id { get; set; }

		public int adTimes { get; set; }

		public int adCountDown { get; set; }

		public string languageId { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.adTimes = base.readInt();
			this.adCountDown = base.readInt();
			this.languageId = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Shop_Ad();
		}
	}
}
