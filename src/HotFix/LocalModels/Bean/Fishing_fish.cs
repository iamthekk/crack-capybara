using System;

namespace LocalModels.Bean
{
	public class Fishing_fish : BaseLocalBean
	{
		public int id { get; set; }

		public string nameId { get; set; }

		public string notes { get; set; }

		public int atlas { get; set; }

		public string icon { get; set; }

		public int area { get; set; }

		public int type { get; set; }

		public int number { get; set; }

		public int weight { get; set; }

		public int[] weightFloat { get; set; }

		public int initialDamage { get; set; }

		public int strength { get; set; }

		public int speed { get; set; }

		public int[] actionList { get; set; }

		public string attributes { get; set; }

		public int skillBuild { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.nameId = base.readLocalString();
			this.notes = base.readLocalString();
			this.atlas = base.readInt();
			this.icon = base.readLocalString();
			this.area = base.readInt();
			this.type = base.readInt();
			this.number = base.readInt();
			this.weight = base.readInt();
			this.weightFloat = base.readArrayint();
			this.initialDamage = base.readInt();
			this.strength = base.readInt();
			this.speed = base.readInt();
			this.actionList = base.readArrayint();
			this.attributes = base.readLocalString();
			this.skillBuild = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Fishing_fish();
		}
	}
}
