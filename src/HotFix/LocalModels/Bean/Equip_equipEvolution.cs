using System;

namespace LocalModels.Bean
{
	public class Equip_equipEvolution : BaseLocalBean
	{
		public int id { get; set; }

		public int evolutionLevel { get; set; }

		public int nextID { get; set; }

		public string[] evolutionCost { get; set; }

		public long[] evolutionAttributes { get; set; }

		public int talentLimit { get; set; }

		public int maxLevel { get; set; }

		public long[] upgradeAttributes { get; set; }

		public string[] powerLvl { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.evolutionLevel = base.readInt();
			this.nextID = base.readInt();
			this.evolutionCost = base.readArraystring();
			this.evolutionAttributes = base.readArraylong();
			this.talentLimit = base.readInt();
			this.maxLevel = base.readInt();
			this.upgradeAttributes = base.readArraylong();
			this.powerLvl = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Equip_equipEvolution();
		}
	}
}
