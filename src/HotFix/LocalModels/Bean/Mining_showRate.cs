using System;

namespace LocalModels.Bean
{
	public class Mining_showRate : BaseLocalBean
	{
		public int id { get; set; }

		public int atlas { get; set; }

		public string icon { get; set; }

		public string languageId { get; set; }

		public int showRate { get; set; }

		public int getRate { get; set; }

		public int quality { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.atlas = base.readInt();
			this.icon = base.readLocalString();
			this.languageId = base.readLocalString();
			this.showRate = base.readInt();
			this.getRate = base.readInt();
			this.quality = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Mining_showRate();
		}
	}
}
