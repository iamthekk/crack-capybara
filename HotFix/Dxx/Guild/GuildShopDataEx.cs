using System;
using LocalModels.Bean;

namespace Dxx.Guild
{
	public static class GuildShopDataEx
	{
		public static GuildItemData GetShowItem(this GuildShopData data)
		{
			if (data.Rewards == null || data.Rewards.Count <= 0)
			{
				return null;
			}
			return data.Rewards[0];
		}

		public static GuildShopType GetShopType(this GuildShopData data)
		{
			Guild_guildShop guildShopTable = GuildProxy.Table.GetGuildShopTable(data.ShopID);
			if (guildShopTable == null)
			{
				return GuildShopType.Daily;
			}
			return (GuildShopType)guildShopTable.Type;
		}
	}
}
