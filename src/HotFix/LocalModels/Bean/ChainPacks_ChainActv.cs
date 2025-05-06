using System;

namespace LocalModels.Bean
{
	public class ChainPacks_ChainActv : BaseLocalBean
	{
		public int id { get; set; }

		public int type { get; set; }

		public int groupID { get; set; }

		public string name { get; set; }

		public string OpenTime { get; set; }

		public string EndTime { get; set; }

		public int condition { get; set; }

		public int RateValue { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.type = base.readInt();
			this.groupID = base.readInt();
			this.name = base.readLocalString();
			this.OpenTime = base.readLocalString();
			this.EndTime = base.readLocalString();
			this.condition = base.readInt();
			this.RateValue = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ChainPacks_ChainActv();
		}
	}
}
