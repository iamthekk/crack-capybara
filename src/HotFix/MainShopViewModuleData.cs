using System;

namespace HotFix
{
	public struct MainShopViewModuleData
	{
		public ShopType ShopTag { readonly get; private set; }

		public MainShopViewModuleData(ShopType targetTag)
		{
			this.ShopTag = targetTag;
		}
	}
}
