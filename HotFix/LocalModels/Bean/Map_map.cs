using System;

namespace LocalModels.Bean
{
	public class Map_map : BaseLocalBean
	{
		public int id { get; set; }

		public int mapType { get; set; }

		public int bottomType { get; set; }

		public string bottomPointName { get; set; }

		public string nameId { get; set; }

		public float[] playerOffset { get; set; }

		public string[] petOffset { get; set; }

		public string[] skyIDs { get; set; }

		public float skyOffsetY { get; set; }

		public string[] bgIds { get; set; }

		public float bgOffsetY { get; set; }

		public float[] bgSpeed { get; set; }

		public string[] farIDs { get; set; }

		public float farOffsetY { get; set; }

		public float[] farSpeed { get; set; }

		public string[] middleIDs { get; set; }

		public float middleOffsetY { get; set; }

		public string[] nearIDs { get; set; }

		public float nearOffsetY { get; set; }

		public float waveOffsetY { get; set; }

		public int[] normalWaves { get; set; }

		public int[] specialWaves { get; set; }

		public int[] clouds { get; set; }

		public float cloudOffsetY { get; set; }

		public float[] cloudSpeed { get; set; }

		public int[] farFloating { get; set; }

		public float farFloatingOffsetY { get; set; }

		public float[] farFloatingSpeed { get; set; }

		public int[] farStatic { get; set; }

		public float farStaticOffsetY { get; set; }

		public int[] middleFloating { get; set; }

		public float middleFloatingOffsetY { get; set; }

		public int[] nearFloating { get; set; }

		public float nearFloatingOffsetY { get; set; }

		public float[] nearFloatingSpeed { get; set; }

		public int[] floatingRandom { get; set; }

		public int isPlanet { get; set; }

		public float[] planetOffset { get; set; }

		public float hangupOffsetY { get; set; }

		public float hangupPlayerY { get; set; }

		public float[] memberColor { get; set; }

		public float[] spriteColor { get; set; }

		public int changeTime { get; set; }

		public string matPrefix { get; set; }

		public int activityMap { get; set; }

		public string activityLightColor { get; set; }

		public string activityStarColor { get; set; }

		public int activityButtonBg { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.mapType = base.readInt();
			this.bottomType = base.readInt();
			this.bottomPointName = base.readLocalString();
			this.nameId = base.readLocalString();
			this.playerOffset = base.readArrayfloat();
			this.petOffset = base.readArraystring();
			this.skyIDs = base.readArraystring();
			this.skyOffsetY = base.readFloat();
			this.bgIds = base.readArraystring();
			this.bgOffsetY = base.readFloat();
			this.bgSpeed = base.readArrayfloat();
			this.farIDs = base.readArraystring();
			this.farOffsetY = base.readFloat();
			this.farSpeed = base.readArrayfloat();
			this.middleIDs = base.readArraystring();
			this.middleOffsetY = base.readFloat();
			this.nearIDs = base.readArraystring();
			this.nearOffsetY = base.readFloat();
			this.waveOffsetY = base.readFloat();
			this.normalWaves = base.readArrayint();
			this.specialWaves = base.readArrayint();
			this.clouds = base.readArrayint();
			this.cloudOffsetY = base.readFloat();
			this.cloudSpeed = base.readArrayfloat();
			this.farFloating = base.readArrayint();
			this.farFloatingOffsetY = base.readFloat();
			this.farFloatingSpeed = base.readArrayfloat();
			this.farStatic = base.readArrayint();
			this.farStaticOffsetY = base.readFloat();
			this.middleFloating = base.readArrayint();
			this.middleFloatingOffsetY = base.readFloat();
			this.nearFloating = base.readArrayint();
			this.nearFloatingOffsetY = base.readFloat();
			this.nearFloatingSpeed = base.readArrayfloat();
			this.floatingRandom = base.readArrayint();
			this.isPlanet = base.readInt();
			this.planetOffset = base.readArrayfloat();
			this.hangupOffsetY = base.readFloat();
			this.hangupPlayerY = base.readFloat();
			this.memberColor = base.readArrayfloat();
			this.spriteColor = base.readArrayfloat();
			this.changeTime = base.readInt();
			this.matPrefix = base.readLocalString();
			this.activityMap = base.readInt();
			this.activityLightColor = base.readLocalString();
			this.activityStarColor = base.readLocalString();
			this.activityButtonBg = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Map_map();
		}
	}
}
