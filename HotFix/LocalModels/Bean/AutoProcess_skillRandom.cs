using System;

namespace LocalModels.Bean
{
	public class AutoProcess_skillRandom : BaseLocalBean
	{
		public int id { get; set; }

		public int[] Pool { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.Pool = base.readArrayint();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new AutoProcess_skillRandom();
		}
	}
}
