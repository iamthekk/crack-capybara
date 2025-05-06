using System;

namespace Dxx.Guild
{
	public class GuildRaceMember
	{
		public long SortIndex
		{
			get
			{
				if (this.Index > 0)
				{
					return (long)this.Index;
				}
				if (this.UserData == null)
				{
					return long.MaxValue;
				}
				return this.UserData.UserID;
			}
		}

		public bool IsSuperPosition
		{
			get
			{
				return this.Position > GuildRaceBattlePosition.Warrior;
			}
		}

		public bool IsEmptyUser
		{
			get
			{
				return this.UserData == null || this.UserData.UserID == 0L;
			}
		}

		public int Index;

		public GuildRaceBattlePosition Position;

		public GuildUserShareData UserData;

		public int ActivityPoint;

		public int RaceScore;

		public ulong Power;

		public string GuildID;

		public string GuildName;
	}
}
