using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsBuyShopItem : BaseEventArgs
	{
		public int ShopItemId { get; set; }

		public override void Clear()
		{
			this.ShopItemId = 0;
		}
	}
}
