using System;

namespace LocalModels.Bean
{
	public class GameSkill_skillSelect : BaseLocalBean
	{
		public int id { get; set; }

		public string sClassName { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.sClassName = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GameSkill_skillSelect();
		}
	}
}
