using System;

namespace LocalModels.Bean
{
	public class LanguageCN_languagetable : BaseLocalBean
	{
		public string id { get; set; }

		public string chinesesimplified { get; set; }

		public override bool readImpl()
		{
			this.id = base.readLocalString();
			this.chinesesimplified = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new LanguageCN_languagetable();
		}
	}
}
