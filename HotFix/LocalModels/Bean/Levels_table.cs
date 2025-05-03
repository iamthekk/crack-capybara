using System;

namespace LocalModels.Bean
{
	public class Levels_table : BaseLocalBean
	{
		public int id { get; set; }

		public string notes { get; set; }

		public string levelName { get; set; }

		public int titleName { get; set; }

		public int isShow { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.notes = base.readLocalString();
			this.levelName = base.readLocalString();
			this.titleName = base.readInt();
			this.isShow = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Levels_table();
		}
	}
}
