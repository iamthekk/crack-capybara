using System;
using System.Collections.Generic;

namespace HotFix
{
	public class IAPWeeklyPackPanel : IAPTimePackPanel
	{
		public override IAPDiamondsType PanelType
		{
			get
			{
				return IAPDiamondsType.WeeklyPack;
			}
		}

		protected override List<PurchaseCommonData.PurchaseData> GetPurchaseData()
		{
			return base.IAPDataModule.TimePackData.GetWeeklyPurchaseDatas();
		}

		protected override long GetRefreshTime()
		{
			return base.IAPDataModule.TimePackData.GetWeeklyTime();
		}

		protected override string GetRefreshTimeString()
		{
			return Singleton<LanguageManager>.Instance.GetInfoByID("2923", new object[] { base.IAPDataModule.TimePackData.GetWeeklyTimeString() });
		}

		protected override void GetInfoLanguageKey(out int titleKey, out int contextKey)
		{
			titleKey = 2903;
			contextKey = 2952;
		}
	}
}
