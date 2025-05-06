using System;

namespace Dxx.Guild
{
	public class GuildEvent_GuildInfoDataChange : GuildBaseEvent
	{
		public override void Clear()
		{
			this.ChangeInfo = null;
		}

		public GuildChangeInfo ChangeInfo;
	}
}
