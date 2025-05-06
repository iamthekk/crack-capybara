using System;
using System.Collections.Generic;

namespace HotFix
{
	public class IAPDayPackPanel : IAPTimePackPanel
	{
		public override IAPDiamondsType PanelType
		{
			get
			{
				return IAPDiamondsType.DayPack;
			}
		}

		protected override List<PurchaseCommonData.PurchaseData> GetPurchaseData()
		{
			return base.IAPDataModule.TimePackData.GetDailyPurchaseDatas();
		}

		protected override long GetRefreshTime()
		{
			return base.IAPDataModule.TimePackData.GetDailyTime();
		}

		protected override string GetRefreshTimeString()
		{
			return Singleton<LanguageManager>.Instance.GetInfoByID("2923", new object[] { base.IAPDataModule.TimePackData.GetDailyTimeString() });
		}

		protected override void GetInfoLanguageKey(out int titleKey, out int contextKey)
		{
			titleKey = 2902;
			contextKey = 2951;
		}
	}
}
