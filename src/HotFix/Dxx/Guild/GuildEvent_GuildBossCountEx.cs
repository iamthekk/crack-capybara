using System;

namespace Dxx.Guild
{
	public static class GuildEvent_GuildBossCountEx
	{
		public static void SetGoldBuyCount(this GuildEvent_GuildBossCount e, int buycount, int cost, int maxbuycount = -1)
		{
			e.BuyCountDic[GuildBossBuyKind.Gold] = new GuildBossBuyCountData
			{
				BuyCount = buycount,
				BuyCost = new GuildItemData
				{
					id = 1,
					count = cost
				},
				MaxBuyCount = maxbuycount,
				BuyKind = GuildBossBuyKind.Gold
			};
		}

		public static void SetDiamondsBuyCount(this GuildEvent_GuildBossCount e, int buycount, int cost, int maxbuycount = -1)
		{
			e.BuyCountDic[GuildBossBuyKind.Diamonds] = new GuildBossBuyCountData
			{
				BuyCount = buycount,
				BuyCost = new GuildItemData
				{
					id = 2,
					count = cost
				},
				MaxBuyCount = maxbuycount,
				BuyKind = GuildBossBuyKind.Diamonds
			};
		}
	}
}
