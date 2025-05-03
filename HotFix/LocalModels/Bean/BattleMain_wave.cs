using System;

namespace LocalModels.Bean
{
	public class BattleMain_wave : BaseLocalBean
	{
		public int ID { get; set; }

		public int roomType { get; set; }

		public int position0 { get; set; }

		public int position1 { get; set; }

		public int position2 { get; set; }

		public int attackUpgrade { get; set; }

		public int hpUpgrade { get; set; }

		public string[] attributes { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.roomType = base.readInt();
			this.position0 = base.readInt();
			this.position1 = base.readInt();
			this.position2 = base.readInt();
			this.attackUpgrade = base.readInt();
			this.hpUpgrade = base.readInt();
			this.attributes = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new BattleMain_wave();
		}
	}
}
