using System;
using System.Collections.Generic;

namespace HotFix
{
	public class MainShopConst
	{
		private const string ShopPanelBasePath = "Assets/_Resources/Prefab/UI/MainShop";

		public static readonly Dictionary<MainShopType, string> MainShopPanelPathDic = new Dictionary<MainShopType, string>
		{
			{
				MainShopType.EquipShop,
				"Assets/_Resources/Prefab/UI/MainShop/EquipShopPanel.prefab"
			},
			{
				MainShopType.BlackMarket,
				"Assets/_Resources/Prefab/UI/MainShop/BlackMarketPanel.prefab"
			},
			{
				MainShopType.SuperValue,
				"Assets/_Resources/Prefab/UI/MainShop/SuperValuePanel.prefab"
			},
			{
				MainShopType.GiftShop,
				"Assets/_Resources/Prefab/UI/MainShop/GiftShopPanel.prefab"
			},
			{
				MainShopType.DiamondShop,
				"Assets/_Resources/Prefab/UI/MainShop/DiamondShopPanel.prefab"
			},
			{
				MainShopType.GuildShop,
				"Assets/_Resources/Prefab/UI/MainShop/GuildShopPanel.prefab"
			},
			{
				MainShopType.ManaCrystalShop,
				"Assets/_Resources/Prefab/UI/MainShop/ManaCrystalShop.prefab"
			}
		};
	}
}
