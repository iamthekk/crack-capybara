using System;

namespace LocalModels.Bean
{
	public class Emoji_Emoji : BaseLocalBean
	{
		public int id { get; set; }

		public string notes { get; set; }

		public int group { get; set; }

		public int languageid { get; set; }

		public string path { get; set; }

		public int atlasId { get; set; }

		public string icon { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.notes = base.readLocalString();
			this.group = base.readInt();
			this.languageid = base.readInt();
			this.path = base.readLocalString();
			this.atlasId = base.readInt();
			this.icon = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Emoji_Emoji();
		}
	}
}
