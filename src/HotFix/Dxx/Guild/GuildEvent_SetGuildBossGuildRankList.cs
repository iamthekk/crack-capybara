using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildEvent_SetGuildBossGuildRankList : GuildBaseEvent
	{
		public override void Clear()
		{
			this.RankList.Clear();
		}

		public List<GuildBossGuildRankData> RankList;

		public long MyRank;

		public GuildBossGuildRankData MyRankData;

		public int Type;
	}
}
