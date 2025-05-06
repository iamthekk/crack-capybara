using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildShopGroup
	{
		public List<GuildShopData> ShopList
		{
			get
			{
				return this.mShopList;
			}
		}

		internal void UpdateGuildShopList(List<GuildShopData> shoplist)
		{
			for (int i = 0; i < shoplist.Count; i++)
			{
				bool flag = false;
				for (int j = 0; j < this.mShopList.Count; j++)
				{
					if (shoplist[i].ShopID == this.mShopList[j].ShopID)
					{
						this.mShopList[j] = shoplist[i];
						flag = true;
						break;
					}
				}
				if (flag)
				{
					shoplist.RemoveAt(i);
					i--;
				}
			}
			for (int k = 0; k < shoplist.Count; k++)
			{
				this.mShopList.Add(shoplist[k]);
			}
		}

		public int GuildShopType;

		private List<GuildShopData> mShopList = new List<GuildShopData>();

		public ulong ShopRefreshTime;

		public GuildItemData RefreshShopCost;
	}
}
