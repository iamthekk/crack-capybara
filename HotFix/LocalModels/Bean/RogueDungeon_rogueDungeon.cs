using System;

namespace LocalModels.Bean
{
	public class RogueDungeon_rogueDungeon : BaseLocalBean
	{
		public int id { get; set; }

		public string name { get; set; }

		public int endEvent { get; set; }

		public int mapID { get; set; }

		public string[] StartAttributeArea { get; set; }

		public string[] AddAttributeArea { get; set; }

		public string[] firstReward { get; set; }

		public string[] firstRewardB { get; set; }

		public string[] playReward { get; set; }

		public string[] playRewardB { get; set; }

		public int[] monsterCfg { get; set; }

		public int eliteCfg { get; set; }

		public int bossCfg { get; set; }

		public int bossDoubleCfg { get; set; }

		public string[] buffData { get; set; }

		public string normalBattleAttr { get; set; }

		public string eliteBattleAttr { get; set; }

		public string bossBattleAttr { get; set; }

		public string[] showMonster { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.name = base.readLocalString();
			this.endEvent = base.readInt();
			this.mapID = base.readInt();
			this.StartAttributeArea = base.readArraystring();
			this.AddAttributeArea = base.readArraystring();
			this.firstReward = base.readArraystring();
			this.firstRewardB = base.readArraystring();
			this.playReward = base.readArraystring();
			this.playRewardB = base.readArraystring();
			this.monsterCfg = base.readArrayint();
			this.eliteCfg = base.readInt();
			this.bossCfg = base.readInt();
			this.bossDoubleCfg = base.readInt();
			this.buffData = base.readArraystring();
			this.normalBattleAttr = base.readLocalString();
			this.eliteBattleAttr = base.readLocalString();
			this.bossBattleAttr = base.readLocalString();
			this.showMonster = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new RogueDungeon_rogueDungeon();
		}
	}
}
