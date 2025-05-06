using System;

namespace LocalModels.Bean
{
	public class Item_battle : BaseLocalBean
	{
		public int id { get; set; }

		public string className { get; set; }

		public int parameter { get; set; }

		public int prefabId { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.className = base.readLocalString();
			this.parameter = base.readInt();
			this.prefabId = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Item_battle();
		}
	}
}
