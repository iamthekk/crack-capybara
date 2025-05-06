using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildEvent_GuildTaskSetData : GuildBaseEvent
	{
		public override void Clear()
		{
		}

		public List<GuildTaskData> TaskList = new List<GuildTaskData>();

		public int DeleteTaskID = -1;

		public int UserDailyActive = -1;

		public int UserWeeklyActive = -1;

		public GuildUpdateInfo UpdateInfo;

		public GuildTaskRefreshData RefreshData;
	}
}
