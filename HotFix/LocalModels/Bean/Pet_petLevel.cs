using System;

namespace LocalModels.Bean
{
	public class Pet_petLevel : BaseLocalBean
	{
		public int id { get; set; }

		public int type { get; set; }

		public int level { get; set; }

		public int talentNeed { get; set; }

		public string[] upgradeAttributes { get; set; }

		public int nextID { get; set; }

		public int levelupFragment { get; set; }

		public string[] levelupCost { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.type = base.readInt();
			this.level = base.readInt();
			this.talentNeed = base.readInt();
			this.upgradeAttributes = base.readArraystring();
			this.nextID = base.readInt();
			this.levelupFragment = base.readInt();
			this.levelupCost = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Pet_petLevel();
		}
	}
}
