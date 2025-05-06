using System;

namespace LocalModels.Bean
{
	public class Relic_relic : BaseLocalBean
	{
		public int id { get; set; }

		public string NameId { get; set; }

		public string DescId { get; set; }

		public int type { get; set; }

		public int bgAtlasID { get; set; }

		public string bgName { get; set; }

		public string baseName { get; set; }

		public int iconAtlasID { get; set; }

		public string iconName { get; set; }

		public int unlockCostID { get; set; }

		public int unlockCostNumber { get; set; }

		public int star { get; set; }

		public int starUpType { get; set; }

		public int AttributesType { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.NameId = base.readLocalString();
			this.DescId = base.readLocalString();
			this.type = base.readInt();
			this.bgAtlasID = base.readInt();
			this.bgName = base.readLocalString();
			this.baseName = base.readLocalString();
			this.iconAtlasID = base.readInt();
			this.iconName = base.readLocalString();
			this.unlockCostID = base.readInt();
			this.unlockCostNumber = base.readInt();
			this.star = base.readInt();
			this.starUpType = base.readInt();
			this.AttributesType = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Relic_relic();
		}
	}
}
