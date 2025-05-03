using System;

namespace LocalModels.Bean
{
	public class Artifact_artifactStage : BaseLocalBean
	{
		public int id { get; set; }

		public int stage { get; set; }

		public int model { get; set; }

		public int itemId { get; set; }

		public string attribute { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.stage = base.readInt();
			this.model = base.readInt();
			this.itemId = base.readInt();
			this.attribute = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Artifact_artifactStage();
		}
	}
}
