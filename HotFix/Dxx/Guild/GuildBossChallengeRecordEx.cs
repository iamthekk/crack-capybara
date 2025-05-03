using System;

namespace Dxx.Guild
{
	public static class GuildBossChallengeRecordEx
	{
		public static string GetNick(this GuildBossChallengeRecord data)
		{
			if (string.IsNullOrEmpty(data.nickName))
			{
				return GuildProxy.GameUser.GetPlayerDefaultNick(data.userId);
			}
			return data.nickName;
		}
	}
}
