using System;

namespace LocalModels.Bean
{
	public class IntegralShop_goods : BaseLocalBean
	{
		public int ID { get; set; }

		public int TypeId { get; set; }

		public int GroupId { get; set; }

		public int WeightInGroup { get; set; }

		public int[] Items { get; set; }

		public int RefreshType { get; set; }

		public int BuyTimes { get; set; }

		public int currencyID { get; set; }

		public int Price { get; set; }

		public int Sort { get; set; }

		public int RequirementType { get; set; }

		public int LevelRequirements { get; set; }

		public int Discount { get; set; }

		public int Hide { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.TypeId = base.readInt();
			this.GroupId = base.readInt();
			this.WeightInGroup = base.readInt();
			this.Items = base.readArrayint();
			this.RefreshType = base.readInt();
			this.BuyTimes = base.readInt();
			this.currencyID = base.readInt();
			this.Price = base.readInt();
			this.Sort = base.readInt();
			this.RequirementType = base.readInt();
			this.LevelRequirements = base.readInt();
			this.Discount = base.readInt();
			this.Hide = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new IntegralShop_goods();
		}
	}
}
