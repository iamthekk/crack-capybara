using System;

namespace LocalModels.Bean
{
	public class GameMember_npcFunction : BaseLocalBean
	{
		public int id { get; set; }

		public float[] npcOffset { get; set; }

		public float[] followOffset { get; set; }

		public float[] fishingStartPos { get; set; }

		public float[] fishingEndPos { get; set; }

		public float[] boxPos { get; set; }

		public int ride { get; set; }

		public float rideScale { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.npcOffset = base.readArrayfloat();
			this.followOffset = base.readArrayfloat();
			this.fishingStartPos = base.readArrayfloat();
			this.fishingEndPos = base.readArrayfloat();
			this.boxPos = base.readArrayfloat();
			this.ride = base.readInt();
			this.rideScale = base.readFloat();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GameMember_npcFunction();
		}
	}
}
