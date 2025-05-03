using System;
using System.Collections.Generic;

namespace HotFix
{
	public class IAPMonthlyPackPanel : IAPTimePackPanel
	{
		public override IAPDiamondsType PanelType
		{
			get
			{
				return IAPDiamondsType.MonthlyPack;
			}
		}

		protected override List<PurchaseCommonData.PurchaseData> GetPurchaseData()
		{
			return base.IAPDataModule.TimePackData.GetMonthPurchaseDatas();
		}

		protected override long GetRefreshTime()
		{
			return base.IAPDataModule.TimePackData.GetMonthTime();
		}

		protected override string GetRefreshTimeString()
		{
			return Singleton<LanguageManager>.Instance.GetInfoByID("2923", new object[] { base.IAPDataModule.TimePackData.GetMonthTimeString() });
		}

		protected override void GetInfoLanguageKey(out int titleKey, out int contextKey)
		{
			titleKey = 2904;
			contextKey = 2953;
		}
	}
}
