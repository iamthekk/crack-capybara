using System;

namespace LocalModels.Bean
{
	public class Item_drop : BaseLocalBean
	{
		public int drop_id { get; set; }

		public int baseOnLv { get; set; }

		public int type { get; set; }

		public string[] reward { get; set; }

		public override bool readImpl()
		{
			this.drop_id = base.readInt();
			this.baseOnLv = base.readInt();
			this.type = base.readInt();
			this.reward = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Item_drop();
		}
	}
}
