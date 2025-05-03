using System;

namespace HotFix
{
	public enum HabbyMailCodeName
	{
		Success,
		ERROR,
		PARAM_ERROR = 20001,
		USER_NOT_FOUND = 20004,
		MAIL_USER_NOT_MATCH = 20301,
		MAIL_NOT_EFFECTIVE,
		MAIL_EXPIRED,
		MAIL_REWARD_RECEIVED,
		SERVER_FATAL_ERROR = 30000,
		SERVER_BUSY,
		GAME_SERVER_ERROR = 40000
	}
}
