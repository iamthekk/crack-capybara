using System;

namespace LocalModels.Bean
{
	public class IntegralShop_data : BaseLocalBean
	{
		public int ID { get; set; }

		public string NameID { get; set; }

		public int currencyID { get; set; }

		public int LevelRequirements { get; set; }

		public int[] RefreshCost { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.NameID = base.readLocalString();
			this.currencyID = base.readInt();
			this.LevelRequirements = base.readInt();
			this.RefreshCost = base.readArrayint();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new IntegralShop_data();
		}
	}
}
