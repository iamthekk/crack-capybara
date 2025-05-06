using System;

namespace LocalModels.Bean
{
	public class BattleMain_skill : BaseLocalBean
	{
		public int ID { get; set; }

		public int skillType { get; set; }

		public int[] skillList { get; set; }

		public int breakSkillId { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.skillType = base.readInt();
			this.skillList = base.readArrayint();
			this.breakSkillId = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new BattleMain_skill();
		}
	}
}
