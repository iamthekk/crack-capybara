using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildEvent_GuildShopSetData : GuildBaseEvent
	{
		public override void Clear()
		{
			this.ShopType = 0;
			this.ShopDataList = null;
		}

		public int ShopType;

		public List<GuildShopData> ShopDataList;
	}
}
