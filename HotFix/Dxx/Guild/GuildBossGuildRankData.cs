using System;

namespace Dxx.Guild
{
	public class GuildBossGuildRankData
	{
		public string GuildID
		{
			get
			{
				if (this.GuildData != null)
				{
					return this.GuildData.GuildID;
				}
				return "0";
			}
		}

		public ulong GuildID_ULong
		{
			get
			{
				if (this.GuildData != null)
				{
					return this.GuildData.GuildID_ULong;
				}
				return 0UL;
			}
		}

		public int Rank;

		public GuildShareSimpleData GuildData;
	}
}
