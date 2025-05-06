using System;

namespace LocalModels.Bean
{
	public class GameSkill_skill : BaseLocalBean
	{
		public int id { get; set; }

		public string nameID { get; set; }

		public string infoID { get; set; }

		public string infoDetailID { get; set; }

		public string fullDetailID { get; set; }

		public int iconAtlasID { get; set; }

		public string icon { get; set; }

		public string iconBadge { get; set; }

		public int isShowInfoHUD { get; set; }

		public int prefabID { get; set; }

		public int startPrefabID { get; set; }

		public string startPosID { get; set; }

		public int typeID { get; set; }

		public string parameters { get; set; }

		public int[] skillTypeDamageAddParam { get; set; }

		public int freedType { get; set; }

		public int tag { get; set; }

		public int initCD { get; set; }

		public int CD { get; set; }

		public int moveType { get; set; }

		public string moveParam { get; set; }

		public int animID { get; set; }

		public string[] animEventNodes { get; set; }

		public int[] fireBullets { get; set; }

		public string triggerConditions { get; set; }

		public int[] selectIDs { get; set; }

		public int rangeType { get; set; }

		public int groupSelectMaxCount { get; set; }

		public string hurtAttributes { get; set; }

		public int effectType { get; set; }

		public string baseAttributes { get; set; }

		public string skillStartOwnerAddBuffs { get; set; }

		public string skillEndOwnerAddBuffs { get; set; }

		public string skillStartTargetAddBuffs { get; set; }

		public string skillEndTargetAddBuffs { get; set; }

		public string skillStartFriendAddBuffs { get; set; }

		public string skillEndFriendAddBuffs { get; set; }

		public int recharge { get; set; }

		public int legacyPower { get; set; }

		public int legacyPowerMax { get; set; }

		public int legacyBindmodelId { get; set; }

		public int lagacyAppearFrame { get; set; }

		public int sagecraftType { get; set; }

		public int isBlack { get; set; }

		public int soundID { get; set; }

		public int combat { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.nameID = base.readLocalString();
			this.infoID = base.readLocalString();
			this.infoDetailID = base.readLocalString();
			this.fullDetailID = base.readLocalString();
			this.iconAtlasID = base.readInt();
			this.icon = base.readLocalString();
			this.iconBadge = base.readLocalString();
			this.isShowInfoHUD = base.readInt();
			this.prefabID = base.readInt();
			this.startPrefabID = base.readInt();
			this.startPosID = base.readLocalString();
			this.typeID = base.readInt();
			this.parameters = base.readLocalString();
			this.skillTypeDamageAddParam = base.readArrayint();
			this.freedType = base.readInt();
			this.tag = base.readInt();
			this.initCD = base.readInt();
			this.CD = base.readInt();
			this.moveType = base.readInt();
			this.moveParam = base.readLocalString();
			this.animID = base.readInt();
			this.animEventNodes = base.readArraystring();
			this.fireBullets = base.readArrayint();
			this.triggerConditions = base.readLocalString();
			this.selectIDs = base.readArrayint();
			this.rangeType = base.readInt();
			this.groupSelectMaxCount = base.readInt();
			this.hurtAttributes = base.readLocalString();
			this.effectType = base.readInt();
			this.baseAttributes = base.readLocalString();
			this.skillStartOwnerAddBuffs = base.readLocalString();
			this.skillEndOwnerAddBuffs = base.readLocalString();
			this.skillStartTargetAddBuffs = base.readLocalString();
			this.skillEndTargetAddBuffs = base.readLocalString();
			this.skillStartFriendAddBuffs = base.readLocalString();
			this.skillEndFriendAddBuffs = base.readLocalString();
			this.recharge = base.readInt();
			this.legacyPower = base.readInt();
			this.legacyPowerMax = base.readInt();
			this.legacyBindmodelId = base.readInt();
			this.lagacyAppearFrame = base.readInt();
			this.sagecraftType = base.readInt();
			this.isBlack = base.readInt();
			this.soundID = base.readInt();
			this.combat = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GameSkill_skill();
		}
	}
}
