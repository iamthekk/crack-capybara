using System;

namespace LocalModels.Bean
{
	public class Collection_collectionStar : BaseLocalBean
	{
		public int id { get; set; }

		public int tagId { get; set; }

		public int star { get; set; }

		public string basicAttribute { get; set; }

		public int effectTimes { get; set; }

		public string effectAttributeEx { get; set; }

		public string[] starItemCost { get; set; }

		public int conditionTalentId { get; set; }

		public string[] powerLvl { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.tagId = base.readInt();
			this.star = base.readInt();
			this.basicAttribute = base.readLocalString();
			this.effectTimes = base.readInt();
			this.effectAttributeEx = base.readLocalString();
			this.starItemCost = base.readArraystring();
			this.conditionTalentId = base.readInt();
			this.powerLvl = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Collection_collectionStar();
		}
	}
}
