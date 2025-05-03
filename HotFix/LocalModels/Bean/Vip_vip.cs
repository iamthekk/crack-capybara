using System;

namespace LocalModels.Bean
{
	public class Vip_vip : BaseLocalBean
	{
		public int id { get; set; }

		public int Exp { get; set; }

		public string[] UnlockReward { get; set; }

		public string[] Price { get; set; }

		public string[] PriceOld { get; set; }

		public string Power1 { get; set; }

		public string Power2 { get; set; }

		public string Power3 { get; set; }

		public string Power4 { get; set; }

		public string Power5 { get; set; }

		public string Power6 { get; set; }

		public string Power7 { get; set; }

		public string Power8 { get; set; }

		public string Power9 { get; set; }

		public string Power10 { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.Exp = base.readInt();
			this.UnlockReward = base.readArraystring();
			this.Price = base.readArraystring();
			this.PriceOld = base.readArraystring();
			this.Power1 = base.readLocalString();
			this.Power2 = base.readLocalString();
			this.Power3 = base.readLocalString();
			this.Power4 = base.readLocalString();
			this.Power5 = base.readLocalString();
			this.Power6 = base.readLocalString();
			this.Power7 = base.readLocalString();
			this.Power8 = base.readLocalString();
			this.Power9 = base.readLocalString();
			this.Power10 = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Vip_vip();
		}
	}
}
