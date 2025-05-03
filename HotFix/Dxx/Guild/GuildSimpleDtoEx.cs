using System;
using System.Collections.Generic;
using Proto.Guild;

namespace Dxx.Guild
{
	public static class GuildSimpleDtoEx
	{
		public static GuildShareSimpleData ToGuildShareSimpleData(this GuildSimpleDto guilddto)
		{
			return new GuildShareSimpleData
			{
				GuildID_ULong = guilddto.GuildId,
				GuildID = guilddto.GuildId.ToString(),
				GuildName = guilddto.GuildName,
				Avatar = (int)guilddto.Avatar,
				AvatarFrame = (int)guilddto.AvatarFrame,
				GuildIcon = (int)guilddto.GuildIcon,
				GuildIconBg = (int)guilddto.GuildIconBg,
				GuildLevel = (int)guilddto.Level,
				GuildPower = (long)guilddto.Power,
				UserId = (long)guilddto.UserId,
				NickName = guilddto.NickName,
				GuildDamage = (long)guilddto.Damage
			};
		}

		public static GuildBossGuildRankData ToGuildBossGuildRankData(this GuildSimpleDto guilddto, int rank)
		{
			return new GuildBossGuildRankData
			{
				GuildData = guilddto.ToGuildShareSimpleData(),
				Rank = rank
			};
		}

		public static List<GuildBossGuildRankData> ToGuildBossGuildRankList(this IList<GuildSimpleDto> datalist)
		{
			List<GuildBossGuildRankData> list = new List<GuildBossGuildRankData>();
			for (int i = 0; i < datalist.Count; i++)
			{
				GuildSimpleDto guildSimpleDto = datalist[i];
				if (guildSimpleDto != null)
				{
					GuildBossGuildRankData guildBossGuildRankData = guildSimpleDto.ToGuildBossGuildRankData(i + 1);
					list.Add(guildBossGuildRankData);
				}
			}
			return list;
		}
	}
}
