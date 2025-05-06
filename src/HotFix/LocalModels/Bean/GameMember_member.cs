using System;

namespace LocalModels.Bean
{
	public class GameMember_member : BaseLocalBean
	{
		public int id { get; set; }

		public int modelID { get; set; }

		public string modelSacle { get; set; }

		public int modelEffectId { get; set; }

		public string nameLanguageID { get; set; }

		public int infoLanguageID { get; set; }

		public int iconAtlasID { get; set; }

		public string iconSpriteName { get; set; }

		public int roleType { get; set; }

		public int meatType { get; set; }

		public int aiTypeID { get; set; }

		public string parameters { get; set; }

		public string skillIDs { get; set; }

		public int recharge { get; set; }

		public string baseAttributes { get; set; }

		public int hitEffectID { get; set; }

		public int appearSoundID { get; set; }

		public int hitSoundID { get; set; }

		public int dieSoundID { get; set; }

		public int[] dropExp { get; set; }

		public int[] dropShell { get; set; }

		public int[] dropCoin { get; set; }

		public int initSkinID { get; set; }

		public int isShowRecharge { get; set; }

		public int npcFunction { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.modelID = base.readInt();
			this.modelSacle = base.readLocalString();
			this.modelEffectId = base.readInt();
			this.nameLanguageID = base.readLocalString();
			this.infoLanguageID = base.readInt();
			this.iconAtlasID = base.readInt();
			this.iconSpriteName = base.readLocalString();
			this.roleType = base.readInt();
			this.meatType = base.readInt();
			this.aiTypeID = base.readInt();
			this.parameters = base.readLocalString();
			this.skillIDs = base.readLocalString();
			this.recharge = base.readInt();
			this.baseAttributes = base.readLocalString();
			this.hitEffectID = base.readInt();
			this.appearSoundID = base.readInt();
			this.hitSoundID = base.readInt();
			this.dieSoundID = base.readInt();
			this.dropExp = base.readArrayint();
			this.dropShell = base.readArrayint();
			this.dropCoin = base.readArrayint();
			this.initSkinID = base.readInt();
			this.isShowRecharge = base.readInt();
			this.npcFunction = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GameMember_member();
		}
	}
}
