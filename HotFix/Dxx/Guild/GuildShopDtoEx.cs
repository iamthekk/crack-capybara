using System;
using System.Collections.Generic;
using Proto.Guild;

namespace Dxx.Guild
{
	public static class GuildShopDtoEx
	{
		public static GuildShopData ToGuildShopData(this GuildShopDto shopdto)
		{
			GuildShopData guildShopData = new GuildShopData();
			guildShopData.ShopID = (int)shopdto.ShopId;
			guildShopData.Position = (int)shopdto.Position;
			guildShopData.Count = (int)shopdto.Count;
			guildShopData.MaxBuyCount = (int)shopdto.Limit;
			guildShopData.Cost = new GuildItemData
			{
				rowId = 0L,
				id = (int)shopdto.NeedItemId,
				count = (int)shopdto.NeedItemCount
			};
			guildShopData.FreeCount = (int)shopdto.FreeCnt;
			guildShopData.Rewards = shopdto.Rewards.ToGuildItemList();
			if (guildShopData.Rewards == null)
			{
				guildShopData.Rewards = new List<GuildItemData>();
			}
			guildShopData.Discount = shopdto.Discount / 10f;
			return guildShopData;
		}

		public static List<GuildShopData> ToGuildShopList(this IList<GuildShopDto> shoplist)
		{
			List<GuildShopData> list = new List<GuildShopData>();
			for (int i = 0; i < shoplist.Count; i++)
			{
				list.Add(shoplist[i].ToGuildShopData());
			}
			return list;
		}
	}
}
