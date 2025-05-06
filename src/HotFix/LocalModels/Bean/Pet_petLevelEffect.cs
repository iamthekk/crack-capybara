using System;

namespace LocalModels.Bean
{
	public class Pet_petLevelEffect : BaseLocalBean
	{
		public int id { get; set; }

		public int type { get; set; }

		public int level { get; set; }

		public string levelupDesc { get; set; }

		public string[] playerAttr { get; set; }

		public string[] petAttr { get; set; }

		public int petSkillLevel { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.type = base.readInt();
			this.level = base.readInt();
			this.levelupDesc = base.readLocalString();
			this.playerAttr = base.readArraystring();
			this.petAttr = base.readArraystring();
			this.petSkillLevel = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Pet_petLevelEffect();
		}
	}
}
