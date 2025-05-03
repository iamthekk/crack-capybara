using System;

namespace LocalModels.Bean
{
	public class TalentNew_talentEvolution : BaseLocalBean
	{
		public int id { get; set; }

		public int type { get; set; }

		public string modelID { get; set; }

		public int iconAtlasID { get; set; }

		public string iconSpriteName { get; set; }

		public int exp { get; set; }

		public string evolutionAttributes { get; set; }

		public string attributeGroup { get; set; }

		public int levelLimit { get; set; }

		public long[] levelupCost { get; set; }

		public string stepLanguageId { get; set; }

		public string desc { get; set; }

		public string[] powerLvl { get; set; }

		public string[] powerEvolution { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.type = base.readInt();
			this.modelID = base.readLocalString();
			this.iconAtlasID = base.readInt();
			this.iconSpriteName = base.readLocalString();
			this.exp = base.readInt();
			this.evolutionAttributes = base.readLocalString();
			this.attributeGroup = base.readLocalString();
			this.levelLimit = base.readInt();
			this.levelupCost = base.readArraylong();
			this.stepLanguageId = base.readLocalString();
			this.desc = base.readLocalString();
			this.powerLvl = base.readArraystring();
			this.powerEvolution = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new TalentNew_talentEvolution();
		}
	}
}
