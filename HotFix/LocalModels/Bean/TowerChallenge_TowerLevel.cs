using System;

namespace LocalModels.Bean
{
	public class TowerChallenge_TowerLevel : BaseLocalBean
	{
		public int id { get; set; }

		public int layer { get; set; }

		public int mapID { get; set; }

		public int levelScriptableID { get; set; }

		public string[] reward { get; set; }

		public string[] MemberData { get; set; }

		public string[] BuffData { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.layer = base.readInt();
			this.mapID = base.readInt();
			this.levelScriptableID = base.readInt();
			this.reward = base.readArraystring();
			this.MemberData = base.readArraystring();
			this.BuffData = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new TowerChallenge_TowerLevel();
		}
	}
}
