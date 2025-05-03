using System;

namespace Proto.Chat
{
	public enum MessageType
	{
		Default,
		GuildJoinSuccess = 101,
		GuildUserJoin,
		GuildUserLeave,
		GuildPositionChange,
		GuildBeKickedOut,
		GuildUserBeKickedOut,
		GuildInfoModify,
		GuildApplyJoin = 111,
		GuildTransferPresident = 113,
		GuildCheckTransferPresident,
		ChatGuild = 201,
		ChatGuildShowItem,
		ChatWorld,
		SystemPush = 9527
	}
}
