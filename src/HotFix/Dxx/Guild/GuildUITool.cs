using System;
using System.Collections.Generic;
using LocalModels.Bean;

namespace Dxx.Guild
{
	public static class GuildUITool
	{
		public static List<GuildShareDataEx.LevelChangeData> GetLevelDetailList(int targetLv)
		{
			List<GuildShareDataEx.LevelChangeData> list = new List<GuildShareDataEx.LevelChangeData>();
			Guild_guildLevel guildLevelTable = GuildProxy.Table.GetGuildLevelTable(targetLv);
			if (guildLevelTable == null)
			{
				return list;
			}
			int maxMemberCount = guildLevelTable.MaxMemberCount;
			int num = ((guildLevelTable.MaxPositionCount.Length != 0) ? guildLevelTable.MaxPositionCount[0] : 0);
			if (guildLevelTable.MaxPositionCount.Length > 1)
			{
				int num2 = guildLevelTable.MaxPositionCount[1];
			}
			if (guildLevelTable.ShopItemCount.Length != 0)
			{
				int num3 = guildLevelTable.ShopItemCount[0];
			}
			if (guildLevelTable.ShopItemCount.Length > 1)
			{
				int num4 = guildLevelTable.ShopItemCount[1];
			}
			int num5 = 0;
			int num6 = 0;
			int num7 = targetLv - 1;
			if (num7 > 0)
			{
				Guild_guildLevel guildLevelTable2 = GuildProxy.Table.GetGuildLevelTable(num7);
				num5 = guildLevelTable2.MaxMemberCount;
				num6 = ((guildLevelTable2.MaxPositionCount.Length != 0) ? guildLevelTable2.MaxPositionCount[0] : 0);
			}
			string levelInfo = GuildUITool.GetLevelInfo(targetLv, num5, maxMemberCount, "400169");
			string levelInfo2 = GuildUITool.GetLevelInfo(targetLv, num6, num, "400171");
			if (!string.IsNullOrEmpty(levelInfo))
			{
				list.Add(GuildUITool.CreateLevelChangeData(levelInfo, num5, maxMemberCount, targetLv));
			}
			if (!string.IsNullOrEmpty(levelInfo2))
			{
				list.Add(GuildUITool.CreateLevelChangeData(levelInfo2, num6, num, targetLv));
			}
			return list;
		}

		private static string GetLevelInfo(int currentLv, int lastCount, int currentCount, string languageId)
		{
			string text = "<color=#9ff400ff>{0}</color>";
			if (lastCount < currentCount)
			{
				bool flag = currentLv > 1 && lastCount == 0;
				string text2 = GuildProxy.Language.GetInfoByID(languageId);
				if (flag)
				{
					text2 = string.Format(text, text2);
				}
				return text2;
			}
			return null;
		}

		private static GuildShareDataEx.LevelChangeData CreateLevelChangeData(string info, int lastNum, int curNum, int lv)
		{
			return new GuildShareDataEx.LevelChangeData
			{
				info = info,
				lastNum = lastNum,
				currentNum = curNum,
				lv = lv
			};
		}
	}
}
