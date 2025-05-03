using System;
using Proto.Guild;

namespace Dxx.Guild
{
	public static class GuildUpdateInfoDtoEx
	{
		public static GuildUpdateInfo ToGuildUpdateData(this GuildUpdateInfoDto updateinfo)
		{
			return new GuildUpdateInfo
			{
				GuildActive = (int)updateinfo.Active,
				Level = (int)updateinfo.Level,
				Exp = (int)updateinfo.Exp,
				MaxMemberCount = (int)updateinfo.MaxMembers
			};
		}
	}
}
