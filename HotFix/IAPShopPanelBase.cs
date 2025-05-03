using System;

namespace HotFix
{
	public abstract class IAPShopPanelBase : IAPPanelBase<IAPShopType>
	{
		private protected IAPShopViewModuleLoader ShopLoader { protected get; private set; }

		public void SetShopLoader(IAPShopViewModuleLoader loader)
		{
			this.ShopLoader = loader;
		}
	}
}
