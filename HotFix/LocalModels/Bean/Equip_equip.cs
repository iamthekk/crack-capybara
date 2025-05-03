using System;

namespace LocalModels.Bean
{
	public class Equip_equip : BaseLocalBean
	{
		public int id { get; set; }

		public int rank { get; set; }

		public int Type { get; set; }

		public int subType { get; set; }

		public int tagID { get; set; }

		public string baseAttributes { get; set; }

		public int composeId { get; set; }

		public int skinId { get; set; }

		public string[] powerLvl { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.rank = base.readInt();
			this.Type = base.readInt();
			this.subType = base.readInt();
			this.tagID = base.readInt();
			this.baseAttributes = base.readLocalString();
			this.composeId = base.readInt();
			this.skinId = base.readInt();
			this.powerLvl = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Equip_equip();
		}
	}
}
