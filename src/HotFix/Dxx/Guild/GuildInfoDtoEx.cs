using System;
using System.Collections.Generic;
using Proto.Guild;

namespace Dxx.Guild
{
	public static class GuildInfoDtoEx
	{
		public static GuildShareData ToGuid(this GuildInfoDto svrdto)
		{
			if (svrdto == null)
			{
				return null;
			}
			GuildShareData guildShareData = new GuildShareData();
			guildShareData.GuildID_ULong = svrdto.GuildId;
			guildShareData.GuildID = svrdto.GuildId.ToString();
			guildShareData.GuildShowName = svrdto.GuildName;
			guildShareData.GuildIcon = (int)svrdto.GuildIcon;
			guildShareData.GuildIconBg = (int)svrdto.GuildIconBg;
			guildShareData.GuildMemberCount = (int)svrdto.Members;
			guildShareData.GuildMemberMaxCount = (int)svrdto.MaxMembers;
			guildShareData.GuildActive = svrdto.Active;
			guildShareData.GuildLevel = (int)svrdto.Level;
			guildShareData.GuildExp = (int)svrdto.Exp;
			guildShareData.JoinKind = (GuildJoinKind)svrdto.ApplyType;
			guildShareData.SetJoinCondition(svrdto.ApplyCondition);
			guildShareData.GuildLanguage = (int)svrdto.Language;
			guildShareData.GuildSlogan = svrdto.GuildIntro;
			guildShareData.GuildNotice = svrdto.GuildNotice;
			guildShareData.PresidentUserID = svrdto.GuildPresidentUserId;
			guildShareData.GuildPower = (long)svrdto.TotalPower;
			guildShareData.IsApply = svrdto.IsApply;
			guildShareData.ServerSetPresidentNick = svrdto.GuildPresidentNickName;
			return guildShareData;
		}

		public static List<GuildShareData> ToGuidList(this IList<GuildInfoDto> guildlist)
		{
			List<GuildShareData> list = new List<GuildShareData>();
			for (int i = 0; i < guildlist.Count; i++)
			{
				GuildInfoDto guildInfoDto = guildlist[i];
				if (guildInfoDto != null)
				{
					list.Add(guildInfoDto.ToGuid());
				}
			}
			return list;
		}
	}
}
