using System;

namespace Dxx.Guild
{
	public class GuildEvent_GuildBossSetTransID : GuildBaseEvent
	{
		public override void Clear()
		{
			this.BossBattleTransID = 0UL;
		}

		public ulong BossBattleTransID;
	}
}
