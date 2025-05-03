using System;

namespace LocalModels.Bean
{
	public class Mining_bonusGame : BaseLocalBean
	{
		public int id { get; set; }

		public int unlockLayer { get; set; }

		public int itemid { get; set; }

		public int num { get; set; }

		public int showWeight { get; set; }

		public int getRate { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.unlockLayer = base.readInt();
			this.itemid = base.readInt();
			this.num = base.readInt();
			this.showWeight = base.readInt();
			this.getRate = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Mining_bonusGame();
		}
	}
}
