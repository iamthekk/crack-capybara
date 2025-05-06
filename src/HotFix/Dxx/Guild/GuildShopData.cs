using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildShopData
	{
		public bool IsRealFree
		{
			get
			{
				return this.Cost.id == 0 && this.Count < this.FreeCount;
			}
		}

		public bool IsAdFree
		{
			get
			{
				return this.Cost.id == 0 && this.Count >= this.FreeCount;
			}
		}

		public int ShopID;

		public int Position;

		public int Count;

		public int MaxBuyCount;

		public GuildItemData Cost;

		public int FreeCount;

		public float Discount;

		public List<GuildItemData> Rewards;
	}
}
