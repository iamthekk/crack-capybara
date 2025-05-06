using System;

namespace LocalModels.Bean
{
	public class Pet_petSummon : BaseLocalBean
	{
		public int id { get; set; }

		public int exp { get; set; }

		public string[] probability { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.exp = base.readInt();
			this.probability = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Pet_petSummon();
		}
	}
}
