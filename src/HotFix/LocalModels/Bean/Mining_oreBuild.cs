using System;

namespace LocalModels.Bean
{
	public class Mining_oreBuild : BaseLocalBean
	{
		public int id { get; set; }

		public int oreType { get; set; }

		public int oreResId { get; set; }

		public int[] drop { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.oreType = base.readInt();
			this.oreResId = base.readInt();
			this.drop = base.readArrayint();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Mining_oreBuild();
		}
	}
}
