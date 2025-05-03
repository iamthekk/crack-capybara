using System;

namespace Dxx.Guild
{
	public class GuildEvent_SetSelfUserData : GuildBaseEvent
	{
		public override void Clear()
		{
		}

		public long UserID;

		public string DeviceID;

		public int LanguageID;
	}
}
