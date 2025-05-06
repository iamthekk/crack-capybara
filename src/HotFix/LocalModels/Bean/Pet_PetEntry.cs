using System;

namespace LocalModels.Bean
{
	public class Pet_PetEntry : BaseLocalBean
	{
		public int id { get; set; }

		public string languageName { get; set; }

		public string languageID { get; set; }

		public int quality { get; set; }

		public int entryType { get; set; }

		public int actionType { get; set; }

		public string action { get; set; }

		public string[] actionSpecial { get; set; }

		public int[] attrRange { get; set; }

		public int weight { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.languageName = base.readLocalString();
			this.languageID = base.readLocalString();
			this.quality = base.readInt();
			this.entryType = base.readInt();
			this.actionType = base.readInt();
			this.action = base.readLocalString();
			this.actionSpecial = base.readArraystring();
			this.attrRange = base.readArrayint();
			this.weight = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Pet_PetEntry();
		}
	}
}
