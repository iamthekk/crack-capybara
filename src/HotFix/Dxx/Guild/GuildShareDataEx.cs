using System;
using System.Collections.Generic;
using LocalModels.Bean;

namespace Dxx.Guild
{
	public static class GuildShareDataEx
	{
		private static GuildSDKManager GuildSDK
		{
			get
			{
				return GuildSDKManager.Instance;
			}
		}

		public static string GetJoinTypeString(this GuildShareData data)
		{
			GuildJoinKind joinKind = data.JoinKind;
			if (joinKind == GuildJoinKind.Free)
			{
				return GuildProxy.Language.GetInfoByID("400061");
			}
			if (joinKind != GuildJoinKind.Conditional)
			{
				HLog.LogError(string.Format("GuildShareDataEx.GetConditionString[{0}] error.", data.JoinKind));
				return "";
			}
			return GuildProxy.Language.GetInfoByID("400062");
		}

		public static string GetLanguageString(this GuildShareData data)
		{
			return GuildProxy.Language.GetLanguageNameString(data.GuildLanguage);
		}

		public static string GetPresidentNick(this GuildShareData data)
		{
			if (string.IsNullOrEmpty(data.ServerSetPresidentNick))
			{
				return GuildProxy.GameUser.GetPlayerDefaultNick(data.PresidentUserID);
			}
			return data.ServerSetPresidentNick;
		}

		public static int GetPositionMaxCount(this GuildShareData data, GuildPositionType pos)
		{
			if (pos == GuildPositionType.President)
			{
				return 1;
			}
			int guildLevel = data.GuildLevel;
			Guild_guildLevel guildLevelTable = GuildProxy.Table.GetGuildLevelTable(guildLevel);
			if (guildLevelTable == null)
			{
				HLog.LogError(string.Format("GuildShareDataEx.GetPositionMaxCount[{0}] error.", guildLevel));
				return 0;
			}
			int[] maxPositionCount = guildLevelTable.MaxPositionCount;
			if (pos == GuildPositionType.VicePresident && maxPositionCount.Length >= 1)
			{
				return maxPositionCount[0];
			}
			if (pos == GuildPositionType.Manager && maxPositionCount.Length >= 2)
			{
				return maxPositionCount[0];
			}
			if (pos == GuildPositionType.Member)
			{
				int num = data.GuildMemberMaxCount - 1;
				for (int i = 0; i < maxPositionCount.Length; i++)
				{
					num -= maxPositionCount[i];
				}
				if (num < 0)
				{
					num = 0;
				}
				return num;
			}
			return data.GuildMemberMaxCount;
		}

		public static bool IsNeedManualLevelUp(this GuildShareData data, GuildPositionType pos)
		{
			Guild_guildConst guildConstTable = GuildProxy.Table.GetGuildConstTable(113);
			return guildConstTable != null && guildConstTable.TypeInt == 1;
		}

		public static bool IsCanLevelUp(this GuildShareData data)
		{
			bool flag = true;
			List<GuildShareDataEx.LevelInfoData> levelUpNeedList = data.GetLevelUpNeedList();
			for (int i = 0; i < levelUpNeedList.Count; i++)
			{
				if (levelUpNeedList[i].curProgress < levelUpNeedList[i].needProgress)
				{
					flag = false;
					break;
				}
			}
			return flag;
		}

		public static List<GuildShareDataEx.LevelInfoData> GetLevelUpNeedList(this GuildShareData data)
		{
			int count = GuildProxy.Table.GetGuildLevelTableAll().Count;
			List<GuildShareDataEx.LevelInfoData> list = new List<GuildShareDataEx.LevelInfoData>();
			if (data.GuildLevel == count)
			{
				return list;
			}
			Guild_guildLevel guildLevelTable = GuildProxy.Table.GetGuildLevelTable(GuildShareDataEx.GuildSDK.GuildInfo.GuildData.GuildLevel);
			int num = GuildShareDataEx.GuildSDK.GuildInfo.GuildData.GuildExp;
			if (guildLevelTable != null)
			{
				num = guildLevelTable.Exp;
			}
			list.Add(new GuildShareDataEx.LevelInfoData
			{
				info = GuildProxy.Language.GetInfoByID1("400159", num),
				needProgress = num,
				curProgress = GuildShareDataEx.GuildSDK.GuildInfo.GuildData.GuildExp
			});
			return list;
		}

		public class LevelInfoData
		{
			public string info;

			public int curProgress;

			public int needProgress;
		}

		public class LevelChangeData
		{
			public string info;

			public int lastNum;

			public int currentNum;

			public int lv;
		}
	}
}
