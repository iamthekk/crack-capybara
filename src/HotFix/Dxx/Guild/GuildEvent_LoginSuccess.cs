using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildEvent_LoginSuccess : GuildBaseEvent
	{
		public override void Clear()
		{
			this.IsJoin = false;
			this.MyGuildShareDetail = null;
			this.SignData = null;
			this.ShopList = null;
			this.TaskList = null;
		}

		public bool IsJoin;

		public bool IsLevelUp;

		public GuildBeKickOutInfo BeKickOutInfo;

		public GuildShareDetailData MyGuildShareDetail;

		public GuildSignData SignData;

		public List<GuildShopGroup> ShopList;

		public List<GuildTaskData> TaskList;

		public GuildTaskRefreshData RefreshData;

		public GuildBossInfo Boss = new GuildBossInfo();

		public GuildDonationInfo DonationInfo;

		public int ApplyJoinCount = -1;

		public long QuitGuildTimeStamp = -1L;

		public int DayContributeTimes;

		public int DayAllContributeTimes;
	}
}
