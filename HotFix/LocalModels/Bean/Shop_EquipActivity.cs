using System;

namespace LocalModels.Bean
{
	public class Shop_EquipActivity : BaseLocalBean
	{
		public int id { get; set; }

		public string nameId { get; set; }

		public string descId { get; set; }

		public string miniPityDesc { get; set; }

		public string hardPityDesc { get; set; }

		public int atlasId { get; set; }

		public string iconName { get; set; }

		public int type { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.nameId = base.readLocalString();
			this.descId = base.readLocalString();
			this.miniPityDesc = base.readLocalString();
			this.hardPityDesc = base.readLocalString();
			this.atlasId = base.readInt();
			this.iconName = base.readLocalString();
			this.type = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Shop_EquipActivity();
		}
	}
}
