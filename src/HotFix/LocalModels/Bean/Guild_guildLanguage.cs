using System;

namespace LocalModels.Bean
{
	public class Guild_guildLanguage : BaseLocalBean
	{
		public int ID { get; set; }

		public string Notes { get; set; }

		public string Code { get; set; }

		public string TranslateCode { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.Notes = base.readLocalString();
			this.Code = base.readLocalString();
			this.TranslateCode = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Guild_guildLanguage();
		}
	}
}
