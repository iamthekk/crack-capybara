using System;
using Proto.Guild;

namespace Dxx.Guild
{
	public static class BeKickedOutDtoEx
	{
		public static GuildBeKickOutInfo ToGuildBeKickOutInfo(this BeKickedOutDto infodto)
		{
			return new GuildBeKickOutInfo
			{
				GuildID = infodto.GuildId.ToString(),
				GuildName = infodto.GuildName.ToString(),
				KickUserID = infodto.FromUserId,
				KickUserNick = infodto.FromUserNickName,
				KickUserPos = (GuildPositionType)infodto.FromUserPosition
			};
		}
	}
}
