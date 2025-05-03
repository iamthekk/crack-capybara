using System;

namespace Dxx.Guild
{
	public class GuildEvent_BeKickOutSetData : GuildBaseEvent
	{
		public override void Clear()
		{
			this.Info = null;
		}

		public GuildBeKickOutInfo Info;
	}
}
