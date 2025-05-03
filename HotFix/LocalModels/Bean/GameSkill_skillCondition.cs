using System;

namespace LocalModels.Bean
{
	public class GameSkill_skillCondition : BaseLocalBean
	{
		public int id { get; set; }

		public int type { get; set; }

		public string parameters { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.type = base.readInt();
			this.parameters = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GameSkill_skillCondition();
		}
	}
}
