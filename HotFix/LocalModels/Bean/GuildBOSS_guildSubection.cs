using System;

namespace LocalModels.Bean
{
	public class GuildBOSS_guildSubection : BaseLocalBean
	{
		public int ID { get; set; }

		public int matchgroup { get; set; }

		public int RankLevel { get; set; }

		public int RankStar { get; set; }

		public int PromotionRank { get; set; }

		public int DemoteRank { get; set; }

		public string languageId { get; set; }

		public int atlasId { get; set; }

		public string atlasName { get; set; }

		public string StarIcon { get; set; }

		public string StarIconBg { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.matchgroup = base.readInt();
			this.RankLevel = base.readInt();
			this.RankStar = base.readInt();
			this.PromotionRank = base.readInt();
			this.DemoteRank = base.readInt();
			this.languageId = base.readLocalString();
			this.atlasId = base.readInt();
			this.atlasName = base.readLocalString();
			this.StarIcon = base.readLocalString();
			this.StarIconBg = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GuildBOSS_guildSubection();
		}
	}
}
