using System;

namespace LocalModels.Bean
{
	public class Guild_guildEvent : BaseLocalBean
	{
		public int id { get; set; }

		public int name { get; set; }

		public int eventLanguage { get; set; }

		public string eventAtlas { get; set; }

		public string eventShow { get; set; }

		public int eventSort { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.name = base.readInt();
			this.eventLanguage = base.readInt();
			this.eventAtlas = base.readLocalString();
			this.eventShow = base.readLocalString();
			this.eventSort = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Guild_guildEvent();
		}
	}
}
