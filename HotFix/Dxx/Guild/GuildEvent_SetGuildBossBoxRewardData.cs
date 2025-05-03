using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildEvent_SetGuildBossBoxRewardData : GuildBaseEvent
	{
		public override void Clear()
		{
			this.KillRewardList = null;
		}

		public List<int> KillRewardList;
	}
}
