using System;

namespace LocalModels.Bean
{
	public class ChapterActivity_Battlepass : BaseLocalBean
	{
		public int id { get; set; }

		public string name { get; set; }

		public int type { get; set; }

		public string[] parameter { get; set; }

		public int atlasID { get; set; }

		public string itemIcon { get; set; }

		public string itemNameId { get; set; }

		public string banner { get; set; }

		public string openTime { get; set; }

		public string endTime { get; set; }

		public int group { get; set; }

		public int purchaseId { get; set; }

		public int finalRewardLimit { get; set; }

		public string[] finalQualityRandom { get; set; }

		public int[] finalDrop { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.name = base.readLocalString();
			this.type = base.readInt();
			this.parameter = base.readArraystring();
			this.atlasID = base.readInt();
			this.itemIcon = base.readLocalString();
			this.itemNameId = base.readLocalString();
			this.banner = base.readLocalString();
			this.openTime = base.readLocalString();
			this.endTime = base.readLocalString();
			this.group = base.readInt();
			this.purchaseId = base.readInt();
			this.finalRewardLimit = base.readInt();
			this.finalQualityRandom = base.readArraystring();
			this.finalDrop = base.readArrayint();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ChapterActivity_Battlepass();
		}
	}
}
