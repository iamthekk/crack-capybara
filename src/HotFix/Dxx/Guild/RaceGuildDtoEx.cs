using System;
using System.Collections.Generic;
using Proto.GuildRace;

namespace Dxx.Guild
{
	public static class RaceGuildDtoEx
	{
		public static GuildRaceGuild ToGuildRaceGuild(this RaceGuildDto guilddto)
		{
			GuildRaceGuild guildRaceGuild = new GuildRaceGuild();
			if (guilddto == null)
			{
				guildRaceGuild.ShareData = new GuildShareData();
				guildRaceGuild.ShareData.CloneFrom(GuildSDKManager.Instance.GuildInfo.GuildData);
				guildRaceGuild.RaceDan = 0;
				guildRaceGuild.RaceScore = 0;
				guildRaceGuild.TotalPower = 0UL;
				guildRaceGuild.IsGuildReg = false;
				return guildRaceGuild;
			}
			guildRaceGuild.ShareData = new GuildShareData();
			guildRaceGuild.ShareData.GuildID_ULong = guilddto.GuildId;
			guildRaceGuild.ShareData.GuildID = guilddto.GuildId.ToString();
			guildRaceGuild.ShareData.GuildShowName = guilddto.GuildName;
			guildRaceGuild.ShareData.GuildIcon = (int)guilddto.Avatar;
			guildRaceGuild.ShareData.GuildIconBg = (int)guilddto.AvatarFrame;
			guildRaceGuild.OppGuildID = guilddto.OppGuildId.ToString();
			guildRaceGuild.OppGuildId_Ulong = guilddto.OppGuildId;
			guildRaceGuild.RaceDan = (int)guilddto.Dan;
			guildRaceGuild.RaceScore = guilddto.Score;
			guildRaceGuild.TotalPower = (ulong)guilddto.Power;
			guildRaceGuild.IsGuildReg = true;
			return guildRaceGuild;
		}

		public static List<GuildRaceGuild> ToGuildRaceGuildList(this IList<RaceGuildDto> guilddtos)
		{
			List<GuildRaceGuild> list = new List<GuildRaceGuild>();
			for (int i = 0; i < guilddtos.Count; i++)
			{
				list.Add(guilddtos[i].ToGuildRaceGuild());
			}
			return list;
		}
	}
}
