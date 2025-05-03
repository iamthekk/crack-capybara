using System;

namespace LocalModels.Bean
{
	public class Achievements_Achievements : BaseLocalBean
	{
		public int ID { get; set; }

		public int AchievementsType { get; set; }

		public int AchievementsLevel { get; set; }

		public int AccumulationType { get; set; }

		public int ProgressType { get; set; }

		public int AchievementsNeed { get; set; }

		public string AchievementsDescribe { get; set; }

		public string[] Reward { get; set; }

		public int UnlockNeed { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.AchievementsType = base.readInt();
			this.AchievementsLevel = base.readInt();
			this.AccumulationType = base.readInt();
			this.ProgressType = base.readInt();
			this.AchievementsNeed = base.readInt();
			this.AchievementsDescribe = base.readLocalString();
			this.Reward = base.readArraystring();
			this.UnlockNeed = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Achievements_Achievements();
		}
	}
}
