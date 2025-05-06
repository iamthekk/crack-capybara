using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildEvent_SetMyGuildFeaturesInfo : GuildBaseEvent
	{
		public override void Clear()
		{
			this.SignData = null;
			this.ShopList = null;
			this.TaskList = null;
			this.ApplyJoinCount = -1;
		}

		public GuildSignData SignData;

		public List<GuildShopGroup> ShopList;

		public List<GuildTaskData> TaskList;

		public GuildTaskRefreshData RefreshData;

		public int ApplyJoinCount = -1;

		public int DayContributeTimes;

		public int DayAllContributeTimes;
	}
}
