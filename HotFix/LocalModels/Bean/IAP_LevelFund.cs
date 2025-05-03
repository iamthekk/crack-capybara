using System;

namespace LocalModels.Bean
{
	public class IAP_LevelFund : BaseLocalBean
	{
		public int id { get; set; }

		public int index { get; set; }

		public int groupId { get; set; }

		public int paramType { get; set; }

		public string[] products { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.index = base.readInt();
			this.groupId = base.readInt();
			this.paramType = base.readInt();
			this.products = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new IAP_LevelFund();
		}
	}
}
