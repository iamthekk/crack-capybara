using System;

namespace Dxx.Guild
{
	public class GuildEvent_SetGuildBossInfo : GuildBaseEvent
	{
		public override void Clear()
		{
			this.Info = null;
		}

		public GuildBossInfo Info = new GuildBossInfo();
	}
}
