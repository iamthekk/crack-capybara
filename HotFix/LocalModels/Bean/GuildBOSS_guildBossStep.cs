using System;

namespace LocalModels.Bean
{
	public class GuildBOSS_guildBossStep : BaseLocalBean
	{
		public int BossLevel { get; set; }

		public string BossStepName { get; set; }

		public int BossGradeQuality { get; set; }

		public int BossBgType { get; set; }

		public int BossId { get; set; }

		public string PlayerAttributes { get; set; }

		public string BossAttributes { get; set; }

		public string[] KillReward { get; set; }

		public string[] ChallengeReward { get; set; }

		public override bool readImpl()
		{
			this.BossLevel = base.readInt();
			this.BossStepName = base.readLocalString();
			this.BossGradeQuality = base.readInt();
			this.BossBgType = base.readInt();
			this.BossId = base.readInt();
			this.PlayerAttributes = base.readLocalString();
			this.BossAttributes = base.readLocalString();
			this.KillReward = base.readArraystring();
			this.ChallengeReward = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GuildBOSS_guildBossStep();
		}
	}
}
