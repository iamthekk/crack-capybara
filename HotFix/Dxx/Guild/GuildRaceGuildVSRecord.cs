using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildRaceGuildVSRecord
	{
		public GuildRaceGuildVSRecord()
		{
			object lockObj = GuildRaceGuildVSRecord.LockObj;
			lock (lockObj)
			{
				ulong nextguid = GuildRaceGuildVSRecord.NEXTGUID;
				GuildRaceGuildVSRecord.NEXTGUID = nextguid + 1UL;
				this.GUID = nextguid;
			}
		}

		public string GuildID1
		{
			get
			{
				if (this.Guild1 != null && this.Guild1.ShareData != null)
				{
					return this.Guild1.ShareData.GuildID;
				}
				return "";
			}
		}

		public ulong GuildID1_Ulong
		{
			get
			{
				if (this.Guild1 != null && this.Guild1.ShareData != null)
				{
					return this.Guild1.ShareData.GuildID_ULong;
				}
				return 0UL;
			}
		}

		public string GuildID2
		{
			get
			{
				if (this.Guild2 != null && this.Guild2.ShareData != null)
				{
					return this.Guild2.ShareData.GuildID;
				}
				return "";
			}
		}

		public ulong GuildID2_Ulong
		{
			get
			{
				if (this.Guild2 != null && this.Guild2.ShareData != null)
				{
					return this.Guild2.ShareData.GuildID_ULong;
				}
				return 0UL;
			}
		}

		public void SortResultList()
		{
			this.ResultList.Sort(new Comparison<GuildRaceUserVSRecord>(GuildRaceUserVSRecord.SortByIndex));
		}

		private static ulong NEXTGUID = 1UL;

		private static object LockObj = new object();

		public ulong GUID;

		public GuildRaceGuild Guild1;

		public GuildRaceGuild Guild2;

		public List<GuildRaceUserVSRecord> ResultList = new List<GuildRaceUserVSRecord>();
	}
}
