using System;

namespace LocalModels.Bean
{
	public class ItemResources_itemget : BaseLocalBean
	{
		public int id { get; set; }

		public string[] itemGet { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.itemGet = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ItemResources_itemget();
		}
	}
}
