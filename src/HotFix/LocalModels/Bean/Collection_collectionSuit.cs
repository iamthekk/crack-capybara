using System;

namespace LocalModels.Bean
{
	public class Collection_collectionSuit : BaseLocalBean
	{
		public int id { get; set; }

		public int suitId { get; set; }

		public string suitNameId { get; set; }

		public int conditionType { get; set; }

		public int conditionParam { get; set; }

		public string conditonPrifixTextId { get; set; }

		public int[] collectionId { get; set; }

		public string attributes { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.suitId = base.readInt();
			this.suitNameId = base.readLocalString();
			this.conditionType = base.readInt();
			this.conditionParam = base.readInt();
			this.conditonPrifixTextId = base.readLocalString();
			this.collectionId = base.readArrayint();
			this.attributes = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Collection_collectionSuit();
		}
	}
}
