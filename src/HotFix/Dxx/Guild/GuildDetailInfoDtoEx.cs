using System;
using Proto.Guild;

namespace Dxx.Guild
{
	public static class GuildDetailInfoDtoEx
	{
		public static GuildShareDetailData ToGuildDetailData(this GuildDetailInfoDto svrdto)
		{
			if (svrdto == null)
			{
				return null;
			}
			return new GuildShareDetailData
			{
				GuildID = svrdto.GuildInfoDto.GuildId.ToString(),
				ShareData = svrdto.GuildInfoDto.ToGuid(),
				Members = svrdto.GuildMemberInfoDtos.ToGuidUserList(),
				IMGroupID = svrdto.GuildInfoDto.ImGroupId
			};
		}
	}
}
