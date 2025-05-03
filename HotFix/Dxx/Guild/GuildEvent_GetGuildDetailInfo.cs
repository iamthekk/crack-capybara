using System;

namespace Dxx.Guild
{
	public class GuildEvent_GetGuildDetailInfo : GuildBaseEvent
	{
		public override void Clear()
		{
			this.GuildData = null;
		}

		public GuildShareDetailData GuildData;
	}
}
