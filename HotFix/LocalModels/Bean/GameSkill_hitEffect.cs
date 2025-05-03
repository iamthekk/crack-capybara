using System;

namespace LocalModels.Bean
{
	public class GameSkill_hitEffect : BaseLocalBean
	{
		public int id { get; set; }

		public string roleType_1 { get; set; }

		public string roleType_2 { get; set; }

		public string roleType_3 { get; set; }

		public string roleType_4 { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.roleType_1 = base.readLocalString();
			this.roleType_2 = base.readLocalString();
			this.roleType_3 = base.readLocalString();
			this.roleType_4 = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GameSkill_hitEffect();
		}
	}
}
