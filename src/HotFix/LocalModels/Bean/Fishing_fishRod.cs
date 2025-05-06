using System;

namespace LocalModels.Bean
{
	public class Fishing_fishRod : BaseLocalBean
	{
		public int id { get; set; }

		public string nameId { get; set; }

		public int atlas { get; set; }

		public string icon { get; set; }

		public int type { get; set; }

		public int hp { get; set; }

		public int hpRestore { get; set; }

		public int strength { get; set; }

		public int speed { get; set; }

		public int tiresSpeed { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.nameId = base.readLocalString();
			this.atlas = base.readInt();
			this.icon = base.readLocalString();
			this.type = base.readInt();
			this.hp = base.readInt();
			this.hpRestore = base.readInt();
			this.strength = base.readInt();
			this.speed = base.readInt();
			this.tiresSpeed = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Fishing_fishRod();
		}
	}
}
