using System;

namespace Dxx.Guild
{
	public class GuildEvent_GuildLevelUpSetData : GuildBaseEvent
	{
		public override void Clear()
		{
			this.GuildUpdateInfo = null;
		}

		public GuildUpdateInfo GuildUpdateInfo;
	}
}
