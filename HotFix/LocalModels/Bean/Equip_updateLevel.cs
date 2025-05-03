using System;

namespace LocalModels.Bean
{
	public class Equip_updateLevel : BaseLocalBean
	{
		public int id { get; set; }

		public int level { get; set; }

		public int nextID { get; set; }

		public string[] levelupCost { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.level = base.readInt();
			this.nextID = base.readInt();
			this.levelupCost = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Equip_updateLevel();
		}
	}
}
