using System;

namespace LocalModels.Bean
{
	public class ChapterActivity_ChapterObj : BaseLocalBean
	{
		public int id { get; set; }

		public int group { get; set; }

		public int score { get; set; }

		public int num { get; set; }

		public int rewardType { get; set; }

		public string[] reward { get; set; }

		public int special { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.group = base.readInt();
			this.score = base.readInt();
			this.num = base.readInt();
			this.rewardType = base.readInt();
			this.reward = base.readArraystring();
			this.special = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ChapterActivity_ChapterObj();
		}
	}
}
