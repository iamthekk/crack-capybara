using System;

namespace LocalModels.Bean
{
	public class GameSkill_skillAnimation : BaseLocalBean
	{
		public int id { get; set; }

		public string animationEnterName { get; set; }

		public string animationAttackName { get; set; }

		public string animationExitName { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.animationEnterName = base.readLocalString();
			this.animationAttackName = base.readLocalString();
			this.animationExitName = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GameSkill_skillAnimation();
		}
	}
}
