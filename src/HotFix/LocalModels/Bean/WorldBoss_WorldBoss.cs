using System;

namespace LocalModels.Bean
{
	public class WorldBoss_WorldBoss : BaseLocalBean
	{
		public int id { get; set; }

		public int monsterCfg { get; set; }

		public int mapID { get; set; }

		public int bgm { get; set; }

		public string[] buffData { get; set; }

		public int bossId { get; set; }

		public float uiScale { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.monsterCfg = base.readInt();
			this.mapID = base.readInt();
			this.bgm = base.readInt();
			this.buffData = base.readArraystring();
			this.bossId = base.readInt();
			this.uiScale = base.readFloat();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new WorldBoss_WorldBoss();
		}
	}
}
