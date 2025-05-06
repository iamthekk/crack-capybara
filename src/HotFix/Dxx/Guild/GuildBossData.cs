using System;
using LocalModels.Bean;

namespace Dxx.Guild
{
	public class GuildBossData
	{
		public int BossID
		{
			get
			{
				GuildBOSS_guildBossStep guildBossStepTable = GuildProxy.Table.GetGuildBossStepTable(this.BossStep);
				if (guildBossStepTable == null)
				{
					return 0;
				}
				return guildBossStepTable.BossId;
			}
		}

		public int BossStep;

		public long CurHP;
	}
}
