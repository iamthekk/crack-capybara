using System;

namespace LocalModels.Bean
{
	public class TalentLegacy_talentLegacyNode : BaseLocalBean
	{
		public int id { get; set; }

		public string romeNumber { get; set; }

		public int type { get; set; }

		public int spineID { get; set; }

		public string name { get; set; }

		public int iconId { get; set; }

		public string icon { get; set; }

		public int career { get; set; }

		public string[] pos { get; set; }

		public string[] condition { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.romeNumber = base.readLocalString();
			this.type = base.readInt();
			this.spineID = base.readInt();
			this.name = base.readLocalString();
			this.iconId = base.readInt();
			this.icon = base.readLocalString();
			this.career = base.readInt();
			this.pos = base.readArraystring();
			this.condition = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new TalentLegacy_talentLegacyNode();
		}
	}
}
