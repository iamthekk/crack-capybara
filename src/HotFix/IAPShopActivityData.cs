using System;
using Framework;
using LocalModels.Bean;
using Proto.Common;

namespace HotFix
{
	public class IAPShopActivityData
	{
		public IAPShopActivityData(ShopActDetailDto data)
		{
			this.UpdateData(data);
		}

		public void UpdateData(ShopActDetailDto data)
		{
			this.shopActDetailDto = data;
			this.activityId = (int)data.ShopActivity;
			Shop_ShopActivity elementById = GameApp.Table.GetManager().GetShop_ShopActivityModelInstance().GetElementById(this.activityId);
			if (elementById != null)
			{
				this.activityType = elementById.type;
				this.linkType = elementById.linkType;
				this.linkId = elementById.linkId;
			}
			else
			{
				this.activityType = 0;
			}
			this.startTimestamp = (long)data.StartTime;
			this.endTimestamp = (long)data.EndTime;
		}

		public int activityId;

		public int activityType;

		public long startTimestamp;

		public long endTimestamp;

		public int linkType;

		public int linkId;

		public ShopActDetailDto shopActDetailDto;
	}
}
