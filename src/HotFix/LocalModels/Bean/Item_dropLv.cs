using System;

namespace LocalModels.Bean
{
	public class Item_dropLv : BaseLocalBean
	{
		public int drop_id { get; set; }

		public int[] level { get; set; }

		public string[] reward { get; set; }

		public override bool readImpl()
		{
			this.drop_id = base.readInt();
			this.level = base.readArrayint();
			this.reward = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Item_dropLv();
		}
	}
}
