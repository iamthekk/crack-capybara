using System;

namespace LocalModels.Bean
{
	public class Fishing_fishMove : BaseLocalBean
	{
		public int id { get; set; }

		public int[] strongTime { get; set; }

		public int[] struggleTime { get; set; }

		public int[] tireTime { get; set; }

		public int[] action { get; set; }

		public int[] actionTime { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.strongTime = base.readArrayint();
			this.struggleTime = base.readArrayint();
			this.tireTime = base.readArrayint();
			this.action = base.readArrayint();
			this.actionTime = base.readArrayint();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Fishing_fishMove();
		}
	}
}
