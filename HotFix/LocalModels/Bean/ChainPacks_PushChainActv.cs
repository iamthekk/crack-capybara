using System;

namespace LocalModels.Bean
{
	public class ChainPacks_PushChainActv : BaseLocalBean
	{
		public int id { get; set; }

		public int type { get; set; }

		public int groupID { get; set; }

		public string name { get; set; }

		public int condition { get; set; }

		public int conditionParams { get; set; }

		public int order { get; set; }

		public int totalPay { get; set; }

		public int lastSeconds { get; set; }

		public int RateValue { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.type = base.readInt();
			this.groupID = base.readInt();
			this.name = base.readLocalString();
			this.condition = base.readInt();
			this.conditionParams = base.readInt();
			this.order = base.readInt();
			this.totalPay = base.readInt();
			this.lastSeconds = base.readInt();
			this.RateValue = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ChainPacks_PushChainActv();
		}
	}
}
