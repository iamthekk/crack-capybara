using System;

namespace LocalModels.Bean
{
	public class Shop_SummonPool : BaseLocalBean
	{
		public int id { get; set; }

		public int poolID { get; set; }

		public int type { get; set; }

		public int itemID { get; set; }

		public int num { get; set; }

		public int weight { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.poolID = base.readInt();
			this.type = base.readInt();
			this.itemID = base.readInt();
			this.num = base.readInt();
			this.weight = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Shop_SummonPool();
		}
	}
}
