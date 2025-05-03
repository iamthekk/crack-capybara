using System;

namespace LocalModels.Bean
{
	public class Dungeon_DungeonBase : BaseLocalBean
	{
		public int id { get; set; }

		public string name { get; set; }

		public int keyID { get; set; }

		public int keyLimit { get; set; }

		public int adId { get; set; }

		public int showDropItemId { get; set; }

		public string desId { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.name = base.readLocalString();
			this.keyID = base.readInt();
			this.keyLimit = base.readInt();
			this.adId = base.readInt();
			this.showDropItemId = base.readInt();
			this.desId = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Dungeon_DungeonBase();
		}
	}
}
