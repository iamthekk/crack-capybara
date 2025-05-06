using System;

namespace LocalModels.Bean
{
	public class Dungeon_DungeonLevel : BaseLocalBean
	{
		public int id { get; set; }

		public int dungeonID { get; set; }

		public int level { get; set; }

		public int mapID { get; set; }

		public string[] MemberData { get; set; }

		public string[] MemberAttribute { get; set; }

		public string[] reward { get; set; }

		public string[] showRate { get; set; }

		public int dropID { get; set; }

		public int dropTimes { get; set; }

		public string[] attrTips { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.dungeonID = base.readInt();
			this.level = base.readInt();
			this.mapID = base.readInt();
			this.MemberData = base.readArraystring();
			this.MemberAttribute = base.readArraystring();
			this.reward = base.readArraystring();
			this.showRate = base.readArraystring();
			this.dropID = base.readInt();
			this.dropTimes = base.readInt();
			this.attrTips = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Dungeon_DungeonLevel();
		}
	}
}
