using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildEvent_SetGuildUserBossRankList : GuildBaseEvent
	{
		public override void Clear()
		{
		}

		public List<GuildBossRankData> RankList;
	}
}
