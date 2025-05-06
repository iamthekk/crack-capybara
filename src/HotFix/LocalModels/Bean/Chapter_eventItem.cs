using System;

namespace LocalModels.Bean
{
	public class Chapter_eventItem : BaseLocalBean
	{
		public int id { get; set; }

		public int function { get; set; }

		public string param { get; set; }

		public int stage { get; set; }

		public string languageId { get; set; }

		public int atlas { get; set; }

		public string icon { get; set; }

		public int showUI { get; set; }

		public int isOverlay { get; set; }

		public int costFood { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.function = base.readInt();
			this.param = base.readLocalString();
			this.stage = base.readInt();
			this.languageId = base.readLocalString();
			this.atlas = base.readInt();
			this.icon = base.readLocalString();
			this.showUI = base.readInt();
			this.isOverlay = base.readInt();
			this.costFood = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Chapter_eventItem();
		}
	}
}
