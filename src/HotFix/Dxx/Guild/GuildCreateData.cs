using System;

namespace Dxx.Guild
{
	public class GuildCreateData
	{
		public bool IsSameWithShareData(GuildShareData sharedata)
		{
			return sharedata != null && (this.GuildShowName == sharedata.GuildShowName && this.GuildSlogan == sharedata.GuildSlogan && this.JoinKind == sharedata.JoinKind && this.GuildLogo == sharedata.GuildIcon && this.GuildLogoBG == sharedata.GuildIconBg && (long)this.JoinCondition_Level == sharedata.LevelNeed && this.Language == sharedata.GuildLanguage) && this.GuildNotice == sharedata.GuildNotice;
		}

		public void CloneFromShareData(GuildShareData sharedata)
		{
			if (sharedata == null)
			{
				return;
			}
			this.GuildShowName = sharedata.GuildShowName;
			this.GuildSlogan = sharedata.GuildSlogan;
			this.JoinKind = sharedata.JoinKind;
			this.GuildLogo = sharedata.GuildIcon;
			this.GuildLogoBG = sharedata.GuildIconBg;
			this.JoinCondition_Level = (int)sharedata.LevelNeed;
			this.Language = sharedata.GuildLanguage;
			this.GuildNotice = sharedata.GuildNotice;
		}

		public string GuildShowName;

		public string GuildSlogan;

		public string GuildNotice;

		public int GuildLogo;

		public int GuildLogoBG;

		public GuildJoinKind JoinKind;

		public int JoinCondition_Level;

		public int Language;
	}
}
