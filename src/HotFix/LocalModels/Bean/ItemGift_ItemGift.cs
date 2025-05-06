using System;

namespace LocalModels.Bean
{
	public class ItemGift_ItemGift : BaseLocalBean
	{
		public int id { get; set; }

		public int Type { get; set; }

		public string Rewards { get; set; }

		public int seconds { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.Type = base.readInt();
			this.Rewards = base.readLocalString();
			this.seconds = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ItemGift_ItemGift();
		}
	}
}
