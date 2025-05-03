using System;

namespace LocalModels.Bean
{
	public class Relic_updateLevel : BaseLocalBean
	{
		public int id { get; set; }

		public int TypeId { get; set; }

		public int level { get; set; }

		public int nextID { get; set; }

		public string[] levelupCost { get; set; }

		public string[] Attributes { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.TypeId = base.readInt();
			this.level = base.readInt();
			this.nextID = base.readInt();
			this.levelupCost = base.readArraystring();
			this.Attributes = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Relic_updateLevel();
		}
	}
}
