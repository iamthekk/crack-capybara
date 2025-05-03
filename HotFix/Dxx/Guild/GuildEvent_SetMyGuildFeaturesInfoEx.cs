using System;
using System.Collections.Generic;
using Proto.Guild;

namespace Dxx.Guild
{
	public static class GuildEvent_SetMyGuildFeaturesInfoEx
	{
		public static void SetDataFromServer(this GuildEvent_SetMyGuildFeaturesInfo e, GuildFeaturesDto featuresdto)
		{
			e.SignData = featuresdto.SignInDto.ToGuildSignData();
			e.ShopList = new List<GuildShopGroup>();
			GuildShopGroup guildShopGroup = new GuildShopGroup();
			guildShopGroup.GuildShopType = 1;
			guildShopGroup.ShopRefreshTime = featuresdto.DailyRefreshTime;
			guildShopGroup.ShopList.AddRange(featuresdto.DailyShop.ToGuildShopList());
			e.ShopList.Add(guildShopGroup);
			GuildShopGroup guildShopGroup2 = new GuildShopGroup();
			guildShopGroup2.GuildShopType = 2;
			guildShopGroup2.ShopRefreshTime = featuresdto.WeeklyRefreshTime;
			guildShopGroup2.ShopList.AddRange(featuresdto.WeeklyShop.ToGuildShopList());
			e.ShopList.Add(guildShopGroup2);
			e.TaskList = featuresdto.Tasks.ToGuidTaskList();
			e.RefreshData = new GuildTaskRefreshData
			{
				TaskRefreshCount = (int)featuresdto.TaskRefreshCount,
				TaskRefreshMaxCount = (int)featuresdto.MaxTaskRefreshCount,
				RefreshCost = new GuildItemData
				{
					id = 2,
					count = (int)featuresdto.TaskRefreshCost
				}
			};
			e.ApplyJoinCount = (int)featuresdto.ApplyCount;
			e.DayContributeTimes = (int)featuresdto.DayContributeTimes;
		}
	}
}
