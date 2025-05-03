using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildEvent_RaceSetBaseInfo : GuildBaseEvent
	{
		public override void Clear()
		{
		}

		public void CheckRealSeasonStartTime(ulong seasontotalmin, ulong curservertime)
		{
			if (seasontotalmin <= 60UL)
			{
				HLog.LogError("公会排位赛赛季配表错误，请检查赛季总时间！！！");
				return;
			}
			ulong num = this.SeasonStartTime;
			int num2 = 10000;
			while (num < curservertime && num2 > 0)
			{
				num2--;
				if (num + seasontotalmin >= curservertime)
				{
					break;
				}
				num += seasontotalmin;
			}
			this.SeasonStartTime = num;
			this.SeasonEndTime = this.SeasonStartTime + seasontotalmin;
		}

		public int RaceStage;

		public int SeasonID;

		public ulong SeasonStartTime;

		public ulong SeasonEndTime;

		public bool IsUserApply;

		public GuildRaceGuild MyGuildInfo;

		public List<GuildRaceGuild> AllGuild;
	}
}
