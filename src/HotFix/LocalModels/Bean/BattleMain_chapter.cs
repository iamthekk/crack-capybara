using System;

namespace LocalModels.Bean
{
	public class BattleMain_chapter : BaseLocalBean
	{
		public int ID { get; set; }

		public int waveStartId { get; set; }

		public int waveCount { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.waveStartId = base.readInt();
			this.waveCount = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new BattleMain_chapter();
		}
	}
}
