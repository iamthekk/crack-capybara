using System;

namespace LocalModels.Bean
{
	public class Relic_starUp : BaseLocalBean
	{
		public int id { get; set; }

		public int TypeId { get; set; }

		public int nextID { get; set; }

		public int starLevel { get; set; }

		public int requireLevel { get; set; }

		public int starUpCost { get; set; }

		public string[] starAttributes { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.TypeId = base.readInt();
			this.nextID = base.readInt();
			this.starLevel = base.readInt();
			this.requireLevel = base.readInt();
			this.starUpCost = base.readInt();
			this.starAttributes = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Relic_starUp();
		}
	}
}
