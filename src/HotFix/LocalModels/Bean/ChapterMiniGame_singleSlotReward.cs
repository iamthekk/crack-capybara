using System;

namespace LocalModels.Bean
{
	public class ChapterMiniGame_singleSlotReward : BaseLocalBean
	{
		public int id { get; set; }

		public int score { get; set; }

		public int skillBuildId { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.score = base.readInt();
			this.skillBuildId = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ChapterMiniGame_singleSlotReward();
		}
	}
}
