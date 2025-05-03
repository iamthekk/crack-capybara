using System;

namespace LocalModels.Bean
{
	public class ChapterActivity_RankActivity : BaseLocalBean
	{
		public int id { get; set; }

		public string name { get; set; }

		public int unlockScore { get; set; }

		public string[] parameter { get; set; }

		public int atlasID { get; set; }

		public string itemIcon { get; set; }

		public string itemNameId { get; set; }

		public string bg { get; set; }

		public string openTime { get; set; }

		public string endTime { get; set; }

		public int group { get; set; }

		public int rankID { get; set; }

		public string[] rewardShow { get; set; }

		public int robotNum { get; set; }

		public string robotScore { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.name = base.readLocalString();
			this.unlockScore = base.readInt();
			this.parameter = base.readArraystring();
			this.atlasID = base.readInt();
			this.itemIcon = base.readLocalString();
			this.itemNameId = base.readLocalString();
			this.bg = base.readLocalString();
			this.openTime = base.readLocalString();
			this.endTime = base.readLocalString();
			this.group = base.readInt();
			this.rankID = base.readInt();
			this.rewardShow = base.readArraystring();
			this.robotNum = base.readInt();
			this.robotScore = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ChapterActivity_RankActivity();
		}
	}
}
