using System;

namespace LocalModels.Bean
{
	public class Artifact_advanceArtifact : BaseLocalBean
	{
		public int id { get; set; }

		public int model { get; set; }

		public int itemId { get; set; }

		public int quality { get; set; }

		public int initSkill { get; set; }

		public int maxStarSkill { get; set; }

		public int maxStar { get; set; }

		public int unlockCostId { get; set; }

		public string[] starCost { get; set; }

		public string attribute { get; set; }

		public int[] levelAttribute { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.model = base.readInt();
			this.itemId = base.readInt();
			this.quality = base.readInt();
			this.initSkill = base.readInt();
			this.maxStarSkill = base.readInt();
			this.maxStar = base.readInt();
			this.unlockCostId = base.readInt();
			this.starCost = base.readArraystring();
			this.attribute = base.readLocalString();
			this.levelAttribute = base.readArrayint();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Artifact_advanceArtifact();
		}
	}
}
