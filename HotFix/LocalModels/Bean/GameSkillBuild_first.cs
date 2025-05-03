using System;

namespace LocalModels.Bean
{
	public class GameSkillBuild_first : BaseLocalBean
	{
		public int id { get; set; }

		public int[] skills { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.skills = base.readArrayint();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GameSkillBuild_first();
		}
	}
}
