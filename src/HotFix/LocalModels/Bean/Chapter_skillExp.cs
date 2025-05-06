using System;

namespace LocalModels.Bean
{
	public class Chapter_skillExp : BaseLocalBean
	{
		public int id { get; set; }

		public int expToNext { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.expToNext = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Chapter_skillExp();
		}
	}
}
