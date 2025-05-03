using System;

namespace LocalModels.Bean
{
	public class Mining_miningBase : BaseLocalBean
	{
		public int id { get; set; }

		public int[] floor { get; set; }

		public int gridType { get; set; }

		public string[] treasure { get; set; }

		public int[] bomb { get; set; }

		public string[] oreBuild { get; set; }

		public int normalOre { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.floor = base.readArrayint();
			this.gridType = base.readInt();
			this.treasure = base.readArraystring();
			this.bomb = base.readArrayint();
			this.oreBuild = base.readArraystring();
			this.normalOre = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Mining_miningBase();
		}
	}
}
