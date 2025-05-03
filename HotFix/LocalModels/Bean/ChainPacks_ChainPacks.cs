using System;

namespace LocalModels.Bean
{
	public class ChainPacks_ChainPacks : BaseLocalBean
	{
		public int id { get; set; }

		public int group { get; set; }

		public int condition { get; set; }

		public int PurchaseId { get; set; }

		public string[] rewards { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.group = base.readInt();
			this.condition = base.readInt();
			this.PurchaseId = base.readInt();
			this.rewards = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ChainPacks_ChainPacks();
		}
	}
}
