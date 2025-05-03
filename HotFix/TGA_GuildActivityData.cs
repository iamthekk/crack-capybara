using System;
using Dxx.Guild;
using Framework;

namespace HotFix
{
	public class TGA_GuildActivityData
	{
		private TGA_GuildActivityData()
		{
		}

		public static TGA_GuildActivityData Create()
		{
			TGA_GuildActivityData tga_GuildActivityData = new TGA_GuildActivityData();
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			GuildInfoDataModule guildInfo = GuildSDKManager.Instance.GuildInfo;
			if (guildInfo.GuildData == null)
			{
				HLog.LogError("[TGA_GuildActivityData]guildInfo.GuildData is nulll");
				return null;
			}
			tga_GuildActivityData.GuildID = guildInfo.GuildData.GuildID;
			tga_GuildActivityData.GuildLevel = guildInfo.GuildData.GuildLevel;
			tga_GuildActivityData.GuildExp = guildInfo.GuildData.GuildExp;
			tga_GuildActivityData.GuildPower = guildInfo.GuildData.GuildPower;
			tga_GuildActivityData.GuildMemberCount = guildInfo.GuildData.GuildMemberCount;
			GuildUserShareData memberData = guildInfo.GuildDetailData.GetMemberData(dataModule.userId);
			tga_GuildActivityData.WeekActivity = memberData.WeeklyActive;
			return tga_GuildActivityData;
		}

		public string GuildID;

		public int GuildLevel;

		public int GuildExp;

		public long GuildPower;

		public int GuildMemberCount;

		public int WeekActivity;
	}
}
