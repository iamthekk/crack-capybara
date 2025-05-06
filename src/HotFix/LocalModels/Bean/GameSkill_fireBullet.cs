using System;

namespace LocalModels.Bean
{
	public class GameSkill_fireBullet : BaseLocalBean
	{
		public int id { get; set; }

		public int bulletID { get; set; }

		public int showTpye { get; set; }

		public int bulletStartPosID { get; set; }

		public int bulletEndPosID { get; set; }

		public int bulletStartPrefabID { get; set; }

		public int bulletEndPrefabID { get; set; }

		public int bulletEndTargetPosPrefabID { get; set; }

		public int bulletStartSoundID { get; set; }

		public int bulletHitSoundID { get; set; }

		public int[] hitPrefabIDs { get; set; }

		public string hitPosID { get; set; }

		public int hitEffectID { get; set; }

		public int hitStopType { get; set; }

		public float hitStopDuration { get; set; }

		public float hitStopDistance { get; set; }

		public float hitStopSpeedParam { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.bulletID = base.readInt();
			this.showTpye = base.readInt();
			this.bulletStartPosID = base.readInt();
			this.bulletEndPosID = base.readInt();
			this.bulletStartPrefabID = base.readInt();
			this.bulletEndPrefabID = base.readInt();
			this.bulletEndTargetPosPrefabID = base.readInt();
			this.bulletStartSoundID = base.readInt();
			this.bulletHitSoundID = base.readInt();
			this.hitPrefabIDs = base.readArrayint();
			this.hitPosID = base.readLocalString();
			this.hitEffectID = base.readInt();
			this.hitStopType = base.readInt();
			this.hitStopDuration = base.readFloat();
			this.hitStopDistance = base.readFloat();
			this.hitStopSpeedParam = base.readFloat();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GameSkill_fireBullet();
		}
	}
}
