using System;

namespace LocalModels.Bean
{
	public class Chapter_slotBuild : BaseLocalBean
	{
		public int id { get; set; }

		public int weight { get; set; }

		public int[] param { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.weight = base.readInt();
			this.param = base.readArrayint();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Chapter_slotBuild();
		}
	}
}
