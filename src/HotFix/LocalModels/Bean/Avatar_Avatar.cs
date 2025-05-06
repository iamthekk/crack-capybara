using System;

namespace LocalModels.Bean
{
	public class Avatar_Avatar : BaseLocalBean
	{
		public int id { get; set; }

		public string name { get; set; }

		public int avatarType { get; set; }

		public string[] unlockItemId { get; set; }

		public int atlasId { get; set; }

		public string iconId { get; set; }

		public string titlebg { get; set; }

		public string titleText { get; set; }

		public int type { get; set; }

		public int isHide { get; set; }

		public string getLanguageId { get; set; }

		public int timeLimit { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.name = base.readLocalString();
			this.avatarType = base.readInt();
			this.unlockItemId = base.readArraystring();
			this.atlasId = base.readInt();
			this.iconId = base.readLocalString();
			this.titlebg = base.readLocalString();
			this.titleText = base.readLocalString();
			this.type = base.readInt();
			this.isHide = base.readInt();
			this.getLanguageId = base.readLocalString();
			this.timeLimit = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Avatar_Avatar();
		}
	}
}
