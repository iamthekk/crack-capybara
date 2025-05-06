using System;

namespace LocalModels.Bean
{
	public class HeroLevelup_HeroLevelup : BaseLocalBean
	{
		public int ID { get; set; }

		public string titleName { get; set; }

		public string[] levelUpCost { get; set; }

		public string[] gradeUpCost { get; set; }

		public string[] levelUpRewards { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.titleName = base.readLocalString();
			this.levelUpCost = base.readArraystring();
			this.gradeUpCost = base.readArraystring();
			this.levelUpRewards = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new HeroLevelup_HeroLevelup();
		}
	}
}
