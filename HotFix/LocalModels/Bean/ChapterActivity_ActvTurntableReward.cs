using System;

namespace LocalModels.Bean
{
	public class ChapterActivity_ActvTurntableReward : BaseLocalBean
	{
		public int id { get; set; }

		public int group { get; set; }

		public int score { get; set; }

		public string[] freeReward { get; set; }

		public int special { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.group = base.readInt();
			this.score = base.readInt();
			this.freeReward = base.readArraystring();
			this.special = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ChapterActivity_ActvTurntableReward();
		}
	}
}
