using System;

namespace LocalModels.Bean
{
	public class RogueDungeon_monsterEntry : BaseLocalBean
	{
		public int id { get; set; }

		public int type { get; set; }

		public int actionType { get; set; }

		public string entryParam { get; set; }

		public string nameId { get; set; }

		public string desId { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.type = base.readInt();
			this.actionType = base.readInt();
			this.entryParam = base.readLocalString();
			this.nameId = base.readLocalString();
			this.desId = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new RogueDungeon_monsterEntry();
		}
	}
}
