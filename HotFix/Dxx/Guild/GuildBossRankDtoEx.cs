using System;
using System.Collections.Generic;
using Proto.Guild;

namespace Dxx.Guild
{
	public static class GuildBossRankDtoEx
	{
		public static GuildBossRankData ToGuildBossRank(this GuildBossRankDto infoDto)
		{
			return new GuildBossRankData
			{
				Rank = (int)infoDto.Rank,
				Damage = (long)infoDto.Damage,
				UserData = infoDto.GuildMemberInfo.ToGuildUser()
			};
		}

		public static List<GuildBossRankData> ToGuildUserBossRankList(this IList<GuildBossRankDto> infoDtos)
		{
			List<GuildBossRankData> list = new List<GuildBossRankData>();
			for (int i = 0; i < infoDtos.Count; i++)
			{
				list.Add(infoDtos[i].ToGuildBossRank());
			}
			return list;
		}
	}
}
