using System;

namespace LocalModels.Bean
{
	public class MonsterCfgOld_monsterCfgOld : BaseLocalBean
	{
		public int id { get; set; }

		public int group { get; set; }

		public int battleType { get; set; }

		public int difficult { get; set; }

		public int pos1 { get; set; }

		public int pos2 { get; set; }

		public int pos3 { get; set; }

		public int pos4 { get; set; }

		public int pos5 { get; set; }

		public int enterBattleMode { get; set; }

		public int ride { get; set; }

		public int isDropBox { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.group = base.readInt();
			this.battleType = base.readInt();
			this.difficult = base.readInt();
			this.pos1 = base.readInt();
			this.pos2 = base.readInt();
			this.pos3 = base.readInt();
			this.pos4 = base.readInt();
			this.pos5 = base.readInt();
			this.enterBattleMode = base.readInt();
			this.ride = base.readInt();
			this.isDropBox = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new MonsterCfgOld_monsterCfgOld();
		}
	}
}
