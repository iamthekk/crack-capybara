using System;

namespace LocalModels.Bean
{
	public class Sociality_Report : BaseLocalBean
	{
		public int id { get; set; }

		public int type { get; set; }

		public int atlasID { get; set; }

		public string bgName { get; set; }

		public string iconBgName { get; set; }

		public string iconName { get; set; }

		public string titleLanguage { get; set; }

		public string contentLanguage { get; set; }

		public int playBack { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.type = base.readInt();
			this.atlasID = base.readInt();
			this.bgName = base.readLocalString();
			this.iconBgName = base.readLocalString();
			this.iconName = base.readLocalString();
			this.titleLanguage = base.readLocalString();
			this.contentLanguage = base.readLocalString();
			this.playBack = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Sociality_Report();
		}
	}
}
