using System;
using Framework.EventSystem;
using Proto.Common;

namespace HotFix
{
	public class EventArgsRefreshShopData : BaseEventArgs
	{
		public ShopType ShopType { get; set; }

		public IntegralShopDto ShopInfo { get; set; }

		public override void Clear()
		{
			this.ShopInfo = null;
			this.ShopType = ShopType.Null;
		}
	}
}
