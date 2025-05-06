using System;

namespace Dxx.Guild
{
	public class GuildEvent_UserGuildPositionChg : GuildBaseEvent
	{
		public override void Clear()
		{
			this.UserID = 0L;
			this.FromGuildPosition = 0;
			this.ToGuildPosition = 0;
			this.HandleUserID = 0L;
		}

		public long UserID;

		public int FromGuildPosition;

		public int ToGuildPosition;

		public long HandleUserID;
	}
}
