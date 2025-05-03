using System;

namespace LocalModels.Bean
{
	public class TalentLegacy_talentLegacyEffect : BaseLocalBean
	{
		public int id { get; set; }

		public int level { get; set; }

		public int tagID { get; set; }

		public string unlockDesc { get; set; }

		public string desc { get; set; }

		public string[] levelupCost { get; set; }

		public int levelupTime { get; set; }

		public string[] attributes { get; set; }

		public int[] skills { get; set; }

		public int addSkillSlot { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.level = base.readInt();
			this.tagID = base.readInt();
			this.unlockDesc = base.readLocalString();
			this.desc = base.readLocalString();
			this.levelupCost = base.readArraystring();
			this.levelupTime = base.readInt();
			this.attributes = base.readArraystring();
			this.skills = base.readArrayint();
			this.addSkillSlot = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new TalentLegacy_talentLegacyEffect();
		}
	}
}
