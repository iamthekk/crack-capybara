using System;

namespace Dxx.Guild
{
	public class GuildEvent_GuildSignSetData : GuildBaseEvent
	{
		public override void Clear()
		{
		}

		public GuildSignData SignData;

		public int UserDailyActive;

		public int UserWeeklyActive;

		public GuildUpdateInfo GuildUpdateInfo;
	}
}
