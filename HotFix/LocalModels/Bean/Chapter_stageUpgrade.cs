using System;

namespace LocalModels.Bean
{
	public class Chapter_stageUpgrade : BaseLocalBean
	{
		public int id { get; set; }

		public int attackUpgrade { get; set; }

		public int hpUpgrade { get; set; }

		public string attr { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.attackUpgrade = base.readInt();
			this.hpUpgrade = base.readInt();
			this.attr = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Chapter_stageUpgrade();
		}
	}
}
