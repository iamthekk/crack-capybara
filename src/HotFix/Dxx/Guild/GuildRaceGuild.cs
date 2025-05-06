using System;

namespace Dxx.Guild
{
	public class GuildRaceGuild
	{
		public string GuildID
		{
			get
			{
				if (this.ShareData != null)
				{
					return this.ShareData.GuildID;
				}
				return "0";
			}
		}

		public ulong GuildID_Ulong
		{
			get
			{
				if (this.ShareData != null)
				{
					return this.ShareData.GuildID_ULong;
				}
				return 0UL;
			}
		}

		public static int SortRank(GuildRaceGuild x, GuildRaceGuild y)
		{
			int num = y.RaceScore.CompareTo(x.RaceScore);
			if (num == 0)
			{
				num = y.ShareData.GuildID_ULong.CompareTo(x.ShareData.GuildID_ULong);
			}
			return num;
		}

		public int RaceDan;

		public int RaceScore;

		public ulong TotalPower;

		public GuildShareData ShareData;

		public bool IsGuildReg;

		public string OppGuildID;

		public ulong OppGuildId_Ulong;
	}
}
