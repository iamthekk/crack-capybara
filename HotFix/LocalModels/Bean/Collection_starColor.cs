using System;

namespace LocalModels.Bean
{
	public class Collection_starColor : BaseLocalBean
	{
		public int id { get; set; }

		public int starNumber { get; set; }

		public int atlasID { get; set; }

		public string iconStar { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.starNumber = base.readInt();
			this.atlasID = base.readInt();
			this.iconStar = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Collection_starColor();
		}
	}
}
