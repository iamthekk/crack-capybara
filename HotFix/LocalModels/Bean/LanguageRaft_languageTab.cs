using System;

namespace LocalModels.Bean
{
	public class LanguageRaft_languageTab : BaseLocalBean
	{
		public int id { get; set; }

		public string name { get; set; }

		public int enumId { get; set; }

		public int use { get; set; }

		public int sortId { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.name = base.readLocalString();
			this.enumId = base.readInt();
			this.use = base.readInt();
			this.sortId = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new LanguageRaft_languageTab();
		}
	}
}
