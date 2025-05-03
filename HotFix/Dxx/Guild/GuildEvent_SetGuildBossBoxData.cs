using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildEvent_SetGuildBossBoxData : GuildBaseEvent
	{
		public override void Clear()
		{
			this.BoxList = null;
		}

		public List<GuildBossKillBox> BoxList;
	}
}
