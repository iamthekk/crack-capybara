using System;

namespace HotFix
{
	public class IAPShopJumpTabData
	{
		public IAPShopType ShopType { get; set; }

		public IAPMainSubType MainSubType { get; set; }

		public IAPDiamondsType DiamondsType { get; set; }

		private IAPShopJumpTabData()
		{
		}

		public static IAPShopJumpTabData CreateMain(IAPMainSubType mainSubType)
		{
			return new IAPShopJumpTabData
			{
				ShopType = IAPShopType.Main,
				MainSubType = mainSubType
			};
		}

		public static IAPShopJumpTabData CreateDiamonds(IAPDiamondsType iapDiamondsType)
		{
			return new IAPShopJumpTabData
			{
				ShopType = IAPShopType.Diamonds,
				DiamondsType = iapDiamondsType
			};
		}

		public static IAPShopJumpTabData CreateBattlePass()
		{
			return new IAPShopJumpTabData
			{
				ShopType = IAPShopType.BattlePass
			};
		}

		public static IAPShopJumpTabData CreateMonthCard()
		{
			return new IAPShopJumpTabData
			{
				ShopType = IAPShopType.MonthCard
			};
		}

		public static IAPShopJumpTabData CreateGrowthFund()
		{
			return new IAPShopJumpTabData
			{
				ShopType = IAPShopType.GrowthFund
			};
		}
	}
}
