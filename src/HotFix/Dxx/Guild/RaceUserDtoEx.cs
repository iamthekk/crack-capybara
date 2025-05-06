using System;
using System.Collections.Generic;
using Proto.GuildRace;

namespace Dxx.Guild
{
	public static class RaceUserDtoEx
	{
		public static GuildRaceMember ToGuildRaceUser(this RaceUserDto user)
		{
			GuildRaceMember guildRaceMember = new GuildRaceMember();
			guildRaceMember.UserData = new GuildUserShareData();
			if (user == null)
			{
				return guildRaceMember;
			}
			guildRaceMember.UserData.UserID = user.UserId;
			guildRaceMember.UserData.Avatar = (int)user.Avatar;
			guildRaceMember.UserData.AvatarFrame = (int)user.AvatarFrame;
			guildRaceMember.UserData.ServerID = user.ServerId;
			guildRaceMember.UserData.Power = user.Power;
			guildRaceMember.UserData.ServerSetNick = user.NickName;
			guildRaceMember.GuildID = user.GuildId.ToString();
			guildRaceMember.GuildName = user.GuildName;
			guildRaceMember.RaceScore = (int)user.Score;
			guildRaceMember.Index = (int)user.Seq;
			guildRaceMember.Position = GuildProxy.Table.GuildRaceUserIndexToPosition(guildRaceMember.Index);
			guildRaceMember.ActivityPoint = (int)user.Ap;
			guildRaceMember.Power = user.Power;
			return guildRaceMember;
		}

		public static List<GuildRaceMember> ToGuildRaceUserList(this IList<RaceUserDto> users)
		{
			List<GuildRaceMember> list = new List<GuildRaceMember>();
			for (int i = 0; i < users.Count; i++)
			{
				list.Add(users[i].ToGuildRaceUser());
			}
			return list;
		}
	}
}
