using System;

namespace LocalModels.Bean
{
	public class Fishing_fishing : BaseLocalBean
	{
		public int id { get; set; }

		public int[] fishUp { get; set; }

		public int distanceDefault { get; set; }

		public int distanceFail { get; set; }

		public int area { get; set; }

		public int bait { get; set; }

		public int fishRod { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.fishUp = base.readArrayint();
			this.distanceDefault = base.readInt();
			this.distanceFail = base.readInt();
			this.area = base.readInt();
			this.bait = base.readInt();
			this.fishRod = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Fishing_fishing();
		}
	}
}
