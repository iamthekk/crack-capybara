using System;

namespace LocalModels.Bean
{
	public class Chapter_chapter : BaseLocalBean
	{
		public int id { get; set; }

		public string nameId { get; set; }

		public float dropBase { get; set; }

		public int totalStage { get; set; }

		public int[] difficultStage { get; set; }

		public int[] startEvent { get; set; }

		public string[] eventQueue { get; set; }

		public string[] eventQueueNew { get; set; }

		public string[] eventQueueV1_0_10 { get; set; }

		public int journeyStage { get; set; }

		public string[] journey { get; set; }

		public string[] journeyV1_0_10 { get; set; }

		public int[] normalEvent { get; set; }

		public int[] monsterGroup { get; set; }

		public string attributes { get; set; }

		public string normalBattleAttr { get; set; }

		public string eliteBattleAttr { get; set; }

		public string bossBattleAttr { get; set; }

		public string npcBattleAttr { get; set; }

		public int mapId { get; set; }

		public int[] rewardStage { get; set; }

		public int[] staminaReturn { get; set; }

		public int[] dropID { get; set; }

		public string[] boxBuild { get; set; }

		public int[] bgm { get; set; }

		public int ride { get; set; }

		public int[] cost { get; set; }

		public int battleMaxDropGold { get; set; }

		public int battleAttributeArea { get; set; }

		public int battleSkillArea { get; set; }

		public int goldDrop { get; set; }

		public int gemDrop { get; set; }

		public string[] packageDrop { get; set; }

		public int[] bigBonus { get; set; }

		public int[] smallBonus { get; set; }

		public int[] itemDrop { get; set; }

		public string[] battleGoldDrop { get; set; }

		public string[] journeyGoldDrop { get; set; }

		public int unlockType { get; set; }

		public int[] unlock { get; set; }

		public string legacyTip { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.nameId = base.readLocalString();
			this.dropBase = base.readFloat();
			this.totalStage = base.readInt();
			this.difficultStage = base.readArrayint();
			this.startEvent = base.readArrayint();
			this.eventQueue = base.readArraystring();
			this.eventQueueNew = base.readArraystring();
			this.eventQueueV1_0_10 = base.readArraystring();
			this.journeyStage = base.readInt();
			this.journey = base.readArraystring();
			this.journeyV1_0_10 = base.readArraystring();
			this.normalEvent = base.readArrayint();
			this.monsterGroup = base.readArrayint();
			this.attributes = base.readLocalString();
			this.normalBattleAttr = base.readLocalString();
			this.eliteBattleAttr = base.readLocalString();
			this.bossBattleAttr = base.readLocalString();
			this.npcBattleAttr = base.readLocalString();
			this.mapId = base.readInt();
			this.rewardStage = base.readArrayint();
			this.staminaReturn = base.readArrayint();
			this.dropID = base.readArrayint();
			this.boxBuild = base.readArraystring();
			this.bgm = base.readArrayint();
			this.ride = base.readInt();
			this.cost = base.readArrayint();
			this.battleMaxDropGold = base.readInt();
			this.battleAttributeArea = base.readInt();
			this.battleSkillArea = base.readInt();
			this.goldDrop = base.readInt();
			this.gemDrop = base.readInt();
			this.packageDrop = base.readArraystring();
			this.bigBonus = base.readArrayint();
			this.smallBonus = base.readArrayint();
			this.itemDrop = base.readArrayint();
			this.battleGoldDrop = base.readArraystring();
			this.journeyGoldDrop = base.readArraystring();
			this.unlockType = base.readInt();
			this.unlock = base.readArrayint();
			this.legacyTip = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Chapter_chapter();
		}
	}
}
