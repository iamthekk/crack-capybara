using System;

namespace LocalModels.Bean
{
	public class ChapterMiniGame_slotBase : BaseLocalBean
	{
		public int id { get; set; }

		public int[] slotIds { get; set; }

		public int spinTimes { get; set; }

		public int weight0 { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.slotIds = base.readArrayint();
			this.spinTimes = base.readInt();
			this.weight0 = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ChapterMiniGame_slotBase();
		}
	}
}
