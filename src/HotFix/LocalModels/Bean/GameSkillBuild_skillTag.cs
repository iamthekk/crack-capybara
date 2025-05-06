using System;

namespace LocalModels.Bean
{
	public class GameSkillBuild_skillTag : BaseLocalBean
	{
		public int id { get; set; }

		public int languageId { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.languageId = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GameSkillBuild_skillTag();
		}
	}
}
