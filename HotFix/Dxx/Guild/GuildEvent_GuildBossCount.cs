using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildEvent_GuildBossCount : GuildBaseEvent
	{
		public override void Clear()
		{
			this.ChallengeCount = -1;
			this.NextChallengeRecoverTime = -1L;
			this.BuyCountDic.Clear();
		}

		public int ChallengeCount = -1;

		public long NextChallengeRecoverTime = -1L;

		public Dictionary<GuildBossBuyKind, GuildBossBuyCountData> BuyCountDic = new Dictionary<GuildBossBuyKind, GuildBossBuyCountData>();
	}
}
