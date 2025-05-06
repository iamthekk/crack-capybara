using System;

namespace Dxx.Guild
{
	public class GuildUserShareData
	{
		public void CloneFrom(GuildUserShareData data)
		{
			this.UserID = data.UserID;
			this.ServerSetNick = data.ServerSetNick;
			this.Avatar = data.Avatar;
			this.AvatarFrame = data.AvatarFrame;
			this.Level = data.Level;
			this.Power = data.Power;
			this.LastOnlineTime = data.LastOnlineTime;
			this.GuildPosition = data.GuildPosition;
			this.DailyActive = data.DailyActive;
			this.WeeklyActive = data.WeeklyActive;
			this.ServerID = data.ServerID;
			this.ChapterID = data.ChapterID;
			this.ATK = data.ATK;
			this.HP = data.HP;
			this.ApplyTime = data.ApplyTime;
			this.JoinTime = data.JoinTime;
			this.OriginalData = data.OriginalData;
		}

		public long UserID;

		[Obsolete("应该使用 GetNick()")]
		public string ServerSetNick;

		public int Avatar;

		public int AvatarFrame;

		public uint Level;

		public ulong Power;

		public ulong LastOnlineTime;

		public GuildPositionType GuildPosition;

		public int DailyActive;

		public int WeeklyActive;

		public uint ServerID = 1U;

		public int ChapterID;

		public ulong ATK;

		public ulong HP;

		public ulong ApplyTime;

		public ulong JoinTime;

		public object OriginalData;
	}
}
