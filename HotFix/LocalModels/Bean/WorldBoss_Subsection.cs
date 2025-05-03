using System;

namespace LocalModels.Bean
{
	public class WorldBoss_Subsection : BaseLocalBean
	{
		public int ID { get; set; }

		public int RankLevel { get; set; }

		public int PromotionRank { get; set; }

		public int DemoteRank { get; set; }

		public string languageId { get; set; }

		public int atlasName { get; set; }

		public string atlasId { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.RankLevel = base.readInt();
			this.PromotionRank = base.readInt();
			this.DemoteRank = base.readInt();
			this.languageId = base.readLocalString();
			this.atlasName = base.readInt();
			this.atlasId = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new WorldBoss_Subsection();
		}
	}
}
