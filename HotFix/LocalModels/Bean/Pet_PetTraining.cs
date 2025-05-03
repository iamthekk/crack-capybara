using System;

namespace LocalModels.Bean
{
	public class Pet_PetTraining : BaseLocalBean
	{
		public int id { get; set; }

		public int level { get; set; }

		public int isSpecial { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.level = base.readInt();
			this.isSpecial = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Pet_PetTraining();
		}
	}
}
