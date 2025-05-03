using System;

namespace LocalModels.Bean
{
	public class ChapterActivity_ChapterActivity : BaseLocalBean
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

		public string[] rewardShow { get; set; }

		public int[] modelShow { get; set; }

		public int[] scoreShow { get; set; }

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
			this.rewardShow = base.readArraystring();
			this.modelShow = base.readArrayint();
			this.scoreShow = base.readArrayint();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ChapterActivity_ChapterActivity();
		}
	}
}
