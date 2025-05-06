using System;

namespace LocalModels.Bean
{
	public class LanguageRaft_languagetable : BaseLocalBean
	{
		public string id { get; set; }

		public string english { get; set; }

		public string spanish { get; set; }

		public string chinesesimplified { get; set; }

		public string vietnamese { get; set; }

		public string chinesetraditional { get; set; }

		public string japanese { get; set; }

		public string french { get; set; }

		public string german { get; set; }

		public string italian { get; set; }

		public string dutch { get; set; }

		public string russian { get; set; }

		public string arabic { get; set; }

		public string korean { get; set; }

		public string thai { get; set; }

		public string portuguese { get; set; }

		public string Indonesia { get; set; }

		public override bool readImpl()
		{
			this.id = base.readLocalString();
			this.english = base.readLocalString();
			this.spanish = base.readLocalString();
			this.chinesesimplified = base.readLocalString();
			this.vietnamese = base.readLocalString();
			this.chinesetraditional = base.readLocalString();
			this.japanese = base.readLocalString();
			this.french = base.readLocalString();
			this.german = base.readLocalString();
			this.italian = base.readLocalString();
			this.dutch = base.readLocalString();
			this.russian = base.readLocalString();
			this.arabic = base.readLocalString();
			this.korean = base.readLocalString();
			this.thai = base.readLocalString();
			this.portuguese = base.readLocalString();
			this.Indonesia = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new LanguageRaft_languagetable();
		}
	}
}
