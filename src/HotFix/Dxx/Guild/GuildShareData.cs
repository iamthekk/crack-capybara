using System;

namespace Dxx.Guild
{
	public class GuildShareData
	{
		public void SetJoinCondition(uint condition)
		{
			this.LevelNeed = (long)((ulong)condition);
		}

		public void CloneFrom(GuildShareData shareData)
		{
			this.GuildID = shareData.GuildID;
			this.GuildID_ULong = shareData.GuildID_ULong;
			this.GuildShowName = shareData.GuildShowName;
			this.GuildIcon = shareData.GuildIcon;
			this.GuildIconBg = shareData.GuildIconBg;
			this.GuildLanguage = shareData.GuildLanguage;
			this.GuildLevel = shareData.GuildLevel;
			this.GuildExp = shareData.GuildExp;
			this.GuildSlogan = shareData.GuildSlogan;
			this.GuildNotice = shareData.GuildNotice;
			this.GuildMemberCount = shareData.GuildMemberCount;
			this.GuildMemberMaxCount = shareData.GuildMemberMaxCount;
			this.GuildActive = shareData.GuildActive;
			this.JoinKind = shareData.JoinKind;
			this.GuildPower = shareData.GuildPower;
			this.PowerNeed = shareData.PowerNeed;
			this.LevelNeed = shareData.LevelNeed;
			this.PresidentUserID = shareData.PresidentUserID;
			this.ServerSetPresidentNick = shareData.ServerSetPresidentNick;
		}

		public string GuildID;

		public ulong GuildID_ULong;

		public string GuildShowName;

		public int GuildIcon;

		public int GuildIconBg;

		public int GuildLanguage;

		public int GuildLevel;

		public int GuildExp;

		public string GuildSlogan;

		public string GuildNotice;

		public int GuildMemberCount;

		public int GuildMemberMaxCount;

		public uint GuildActive;

		public long PresidentUserID;

		[Obsolete("应该使用 GetPresidentNick()")]
		public string ServerSetPresidentNick;

		public GuildJoinKind JoinKind;

		public long GuildPower;

		public long PowerNeed;

		public long LevelNeed;

		public bool IsApply;
	}
}
