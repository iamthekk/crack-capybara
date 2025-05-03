using System;

namespace LocalModels.Bean
{
	public class Quality_guildBossQuality : BaseLocalBean
	{
		public int id { get; set; }

		public int atlasId { get; set; }

		public string guildBossGradeBgName { get; set; }

		public string gradeColor { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.atlasId = base.readInt();
			this.guildBossGradeBgName = base.readLocalString();
			this.gradeColor = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Quality_guildBossQuality();
		}
	}
}
