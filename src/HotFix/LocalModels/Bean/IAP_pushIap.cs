using System;

namespace LocalModels.Bean
{
	public class IAP_pushIap : BaseLocalBean
	{
		public int id { get; set; }

		public int type { get; set; }

		public int condition { get; set; }

		public int conditionParams { get; set; }

		public int order { get; set; }

		public int totalPay { get; set; }

		public int group { get; set; }

		public int lastSeconds { get; set; }

		public int PosType { get; set; }

		public string PrefabType { get; set; }

		public string Title { get; set; }

		public string Des { get; set; }

		public string[] products { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.type = base.readInt();
			this.condition = base.readInt();
			this.conditionParams = base.readInt();
			this.order = base.readInt();
			this.totalPay = base.readInt();
			this.group = base.readInt();
			this.lastSeconds = base.readInt();
			this.PosType = base.readInt();
			this.PrefabType = base.readLocalString();
			this.Title = base.readLocalString();
			this.Des = base.readLocalString();
			this.products = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new IAP_pushIap();
		}
	}
}
