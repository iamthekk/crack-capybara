using System;
using Proto.Guild;

namespace Dxx.Guild
{
	public static class GuildBossConfigDtoEx
	{
		public static GuildBossData ToGuildBossData(this GuildBossConfigDto infoDto)
		{
			GuildBossData guildBossData = new GuildBossData();
			if (infoDto != null)
			{
				guildBossData.BossStep = (int)infoDto.BossStep;
				guildBossData.CurHP = infoDto.NowHp;
			}
			return guildBossData;
		}
	}
}
