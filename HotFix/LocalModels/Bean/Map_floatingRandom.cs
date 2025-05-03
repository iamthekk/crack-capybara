using System;

namespace LocalModels.Bean
{
	public class Map_floatingRandom : BaseLocalBean
	{
		public int id { get; set; }

		public int count { get; set; }

		public float minOffsetX { get; set; }

		public float maxOffsetX { get; set; }

		public float minOffsetY { get; set; }

		public float maxOffsetY { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.count = base.readInt();
			this.minOffsetX = base.readFloat();
			this.maxOffsetX = base.readFloat();
			this.minOffsetY = base.readFloat();
			this.maxOffsetY = base.readFloat();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Map_floatingRandom();
		}
	}
}
