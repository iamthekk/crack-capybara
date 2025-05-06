using System;

namespace LocalModels.Bean
{
	public class Chapter_attributeBuild : BaseLocalBean
	{
		public int id { get; set; }

		public int atlasId { get; set; }

		public string icon { get; set; }

		public string attributes { get; set; }

		public int weight { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.atlasId = base.readInt();
			this.icon = base.readLocalString();
			this.attributes = base.readLocalString();
			this.weight = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Chapter_attributeBuild();
		}
	}
}
