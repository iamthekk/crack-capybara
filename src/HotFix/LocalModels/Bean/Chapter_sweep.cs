using System;

namespace LocalModels.Bean
{
	public class Chapter_sweep : BaseLocalBean
	{
		public int id { get; set; }

		public int range { get; set; }

		public int multiplier { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.range = base.readInt();
			this.multiplier = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Chapter_sweep();
		}
	}
}
