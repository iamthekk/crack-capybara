using System;

namespace LocalModels.Bean
{
	public class GameSkillBuild_skillRandom : BaseLocalBean
	{
		public int id { get; set; }

		public int[] weight { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.weight = base.readArrayint();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GameSkillBuild_skillRandom();
		}
	}
}
