using System;

namespace LocalModels.Bean
{
	public class ChapterMiniGame_paySlotBase : BaseLocalBean
	{
		public int id { get; set; }

		public int[] first { get; set; }

		public int[] second { get; set; }

		public int[] third { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.first = base.readArrayint();
			this.second = base.readArrayint();
			this.third = base.readArrayint();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ChapterMiniGame_paySlotBase();
		}
	}
}
