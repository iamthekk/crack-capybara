using System;

namespace LocalModels.Bean
{
	public class ChapterActivity_BattlepassReward : BaseLocalBean
	{
		public int id { get; set; }

		public int group { get; set; }

		public int score { get; set; }

		public int type { get; set; }

		public string[] freeReward { get; set; }

		public string[] payReward { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.group = base.readInt();
			this.score = base.readInt();
			this.type = base.readInt();
			this.freeReward = base.readArraystring();
			this.payReward = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ChapterActivity_BattlepassReward();
		}
	}
}
