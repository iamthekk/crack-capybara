using System;

namespace LocalModels.Bean
{
	public class ItemResources_jumpResource : BaseLocalBean
	{
		public int id { get; set; }

		public int ab { get; set; }

		public int jumpType { get; set; }

		public int jumpId { get; set; }

		public int atlas { get; set; }

		public string icon { get; set; }

		public string language { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.ab = base.readInt();
			this.jumpType = base.readInt();
			this.jumpId = base.readInt();
			this.atlas = base.readInt();
			this.icon = base.readLocalString();
			this.language = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ItemResources_jumpResource();
		}
	}
}
