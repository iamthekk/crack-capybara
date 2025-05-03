using System;

namespace LocalModels.Bean
{
	public class Language_languagetable : BaseLocalBean
	{
		public int id { get; set; }

		public string english { get; set; }

		public string chinesesimplified { get; set; }

		public string chinesetraditional { get; set; }

		public string vietnamese { get; set; }

		public string spanish { get; set; }

		public string japanese { get; set; }

		public string french { get; set; }

		public string german { get; set; }

		public string italian { get; set; }

		public string dutch { get; set; }

		public string russian { get; set; }

		public string arabic { get; set; }

		public string korean { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.english = base.readLocalString();
			this.chinesesimplified = base.readLocalString();
			this.chinesetraditional = base.readLocalString();
			this.vietnamese = base.readLocalString();
			this.spanish = base.readLocalString();
			this.japanese = base.readLocalString();
			this.french = base.readLocalString();
			this.german = base.readLocalString();
			this.italian = base.readLocalString();
			this.dutch = base.readLocalString();
			this.russian = base.readLocalString();
			this.arabic = base.readLocalString();
			this.korean = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Language_languagetable();
		}
	}
}
