using System;

namespace LocalModels.Bean
{
	public class Collection_collection : BaseLocalBean
	{
		public int id { get; set; }

		public int rarity { get; set; }

		public int suitId { get; set; }

		public int tagId { get; set; }

		public int needFragment { get; set; }

		public string toFragment { get; set; }

		public string nameId { get; set; }

		public string descId { get; set; }

		public int passiveType { get; set; }

		public int passivePara { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.rarity = base.readInt();
			this.suitId = base.readInt();
			this.tagId = base.readInt();
			this.needFragment = base.readInt();
			this.toFragment = base.readLocalString();
			this.nameId = base.readLocalString();
			this.descId = base.readLocalString();
			this.passiveType = base.readInt();
			this.passivePara = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Collection_collection();
		}
	}
}
