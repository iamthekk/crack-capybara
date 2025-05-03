using System;

namespace LocalModels.Bean
{
	public class Avatar_SceneSkin : BaseLocalBean
	{
		public int id { get; set; }

		public string name { get; set; }

		public int avatarType { get; set; }

		public string[] unlockItemId { get; set; }

		public string icon { get; set; }

		public string prefabPath { get; set; }

		public int isHide { get; set; }

		public string getLanguageId { get; set; }

		public int timeLimit { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.name = base.readLocalString();
			this.avatarType = base.readInt();
			this.unlockItemId = base.readArraystring();
			this.icon = base.readLocalString();
			this.prefabPath = base.readLocalString();
			this.isHide = base.readInt();
			this.getLanguageId = base.readLocalString();
			this.timeLimit = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Avatar_SceneSkin();
		}
	}
}
