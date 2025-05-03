using System;
using System.Collections.Generic;
using Proto.Guild;

namespace Dxx.Guild
{
	public static class GuildMemberInfoDtoEx
	{
		public static GuildUserShareData ToGuildUser(this GuildMemberInfoDto svrdto)
		{
			return new GuildUserShareData
			{
				UserID = svrdto.UserId,
				ServerSetNick = svrdto.NickName,
				Avatar = (int)svrdto.Avatar,
				AvatarFrame = (int)svrdto.AvatarFrame,
				Level = svrdto.Level,
				LastOnlineTime = svrdto.ActiveTime,
				GuildPosition = (GuildPositionType)svrdto.Position,
				ATK = (ulong)svrdto.Atk,
				HP = (ulong)svrdto.Hp,
				Power = svrdto.BattlePower,
				ApplyTime = svrdto.ApplyTime,
				JoinTime = svrdto.JoinTime,
				ChapterID = (int)svrdto.ChapterId,
				DailyActive = (int)svrdto.DailyActive,
				WeeklyActive = (int)svrdto.WeekActive,
				OriginalData = svrdto
			};
		}

		public static List<GuildUserShareData> ToGuidUserList(this IList<GuildMemberInfoDto> memberlist)
		{
			List<GuildUserShareData> list = new List<GuildUserShareData>();
			for (int i = 0; i < memberlist.Count; i++)
			{
				GuildMemberInfoDto guildMemberInfoDto = memberlist[i];
				if (guildMemberInfoDto != null)
				{
					list.Add(guildMemberInfoDto.ToGuildUser());
				}
			}
			return list;
		}
	}
}
