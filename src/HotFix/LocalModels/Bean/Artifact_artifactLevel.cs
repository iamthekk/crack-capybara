using System;

namespace LocalModels.Bean
{
	public class Artifact_artifactLevel : BaseLocalBean
	{
		public int id { get; set; }

		public int stage { get; set; }

		public int star { get; set; }

		public int itemCost { get; set; }

		public int levelCost { get; set; }

		public string[] attribute { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.stage = base.readInt();
			this.star = base.readInt();
			this.itemCost = base.readInt();
			this.levelCost = base.readInt();
			this.attribute = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Artifact_artifactLevel();
		}
	}
}
