using System;

namespace LocalModels.Bean
{
	public class ChapterActivity_BattlepassQuality : BaseLocalBean
	{
		public int id { get; set; }

		public int upgradeRate { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.upgradeRate = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ChapterActivity_BattlepassQuality();
		}
	}
}
