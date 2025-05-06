using System;

namespace LocalModels.Bean
{
	public class ActivityTurntable_TurntablePay : BaseLocalBean
	{
		public int id { get; set; }

		public int PurchaseId { get; set; }

		public int AdId { get; set; }

		public string ObjName { get; set; }

		public int objToplimit { get; set; }

		public string[] ObjGoods { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.PurchaseId = base.readInt();
			this.AdId = base.readInt();
			this.ObjName = base.readLocalString();
			this.objToplimit = base.readInt();
			this.ObjGoods = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ActivityTurntable_TurntablePay();
		}
	}
}
