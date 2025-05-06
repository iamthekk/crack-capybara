using System;

namespace LocalModels.Bean
{
	public class Pet_pet : BaseLocalBean
	{
		public int id { get; set; }

		public int memberId { get; set; }

		public string nameID { get; set; }

		public int quality { get; set; }

		public int battleSkill { get; set; }

		public string toFragment { get; set; }

		public int levelEffectID { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.memberId = base.readInt();
			this.nameID = base.readLocalString();
			this.quality = base.readInt();
			this.battleSkill = base.readInt();
			this.toFragment = base.readLocalString();
			this.levelEffectID = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Pet_pet();
		}
	}
}
