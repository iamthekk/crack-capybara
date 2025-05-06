using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using LocalModels.Model;

namespace HotFix
{
	public class IAPTimePackData
	{
		public IAPTimePackData(PurchaseCommonData commonDataVal)
		{
			this.commonData = commonDataVal;
			this.counts = new MapField<uint, uint>();
		}

		public void SetData(MapField<uint, uint> buyCounts, ulong dailyTimeVal, ulong weeklyTimeVal, ulong monthTimeVal, bool isClear)
		{
			this.counts = buyCounts;
			this.dailyTime = (long)dailyTimeVal;
			this.weeklyTime = (long)weeklyTimeVal;
			this.monthTime = (long)monthTimeVal;
			if (this.counts == null)
			{
				HLog.LogError(string.Format("{0} {1} {2} is null", this, "SetData", "counts"));
				this.counts = new MapField<uint, uint>();
			}
		}

		public bool IsMaxBuyCount(int id)
		{
			int buyCount = this.GetBuyCount(id);
			IAP_Purchase elementById = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(id);
			return elementById != null && elementById.limitCount != 0 && buyCount >= elementById.limitCount;
		}

		public int GetMaxBuyCount(int id)
		{
			IAP_Purchase elementById = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(id);
			if (elementById == null)
			{
				return 0;
			}
			return elementById.limitCount;
		}

		public int GetBuyCount(int id)
		{
			if (this.counts == null)
			{
				return 0;
			}
			uint num;
			this.counts.TryGetValue((uint)id, ref num);
			return (int)num;
		}

		public bool IsShowRedPoint(int tableID)
		{
			IAP_Purchase elementById = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(tableID);
			return elementById != null && elementById.price1 == 0f && !this.IsMaxBuyCount(tableID);
		}

		private long GetCountDownByPackType(IAPTimePackData.PackType packType)
		{
			switch (packType)
			{
			case IAPTimePackData.PackType.Daily:
				return this.GetDailyTime();
			case IAPTimePackData.PackType.Weekly:
				return this.GetWeeklyTime();
			case IAPTimePackData.PackType.Month:
				return this.GetMonthTime();
			default:
				return 2147483647L;
			}
		}

		public long GetDailyTime()
		{
			return this.GetRefreshTime(this.dailyTime);
		}

		public long GetWeeklyTime()
		{
			return this.GetRefreshTime(this.weeklyTime);
		}

		public long GetMonthTime()
		{
			return this.GetRefreshTime(this.monthTime);
		}

		public long GetRefreshTime(IAPTimePackData.PackType packType)
		{
			switch (packType)
			{
			case IAPTimePackData.PackType.Daily:
				return this.GetDailyTime();
			case IAPTimePackData.PackType.Weekly:
				return this.GetWeeklyTime();
			case IAPTimePackData.PackType.Month:
				return this.GetMonthTime();
			default:
				HLog.LogError(string.Format("GetRefreshTime packType:{0} error", packType));
				return 0L;
			}
		}

		private long GetRefreshTime(long endTime)
		{
			return Utility.Math.Max(endTime - DxxTools.Time.ServerTimestamp, 0L);
		}

		public string GetDailyTimeString()
		{
			return Singleton<LanguageManager>.Instance.GetTime(this.GetDailyTime());
		}

		public string GetFormatTimeStr(long time)
		{
			return Singleton<LanguageManager>.Instance.GetTime(time);
		}

		public string GetWeeklyTimeString()
		{
			return Singleton<LanguageManager>.Instance.GetTime(this.GetWeeklyTime());
		}

		public string GetMonthTimeString()
		{
			return Singleton<LanguageManager>.Instance.GetTime(this.GetMonthTime());
		}

		public List<PurchaseCommonData.PurchaseData> GetDailyPurchaseDatas()
		{
			return this.GetPurchaseData(IAPTimePackData.PackType.Daily);
		}

		public List<PurchaseCommonData.PurchaseData> GetWeeklyPurchaseDatas()
		{
			return this.GetPurchaseData(IAPTimePackData.PackType.Weekly);
		}

		public List<PurchaseCommonData.PurchaseData> GetMonthPurchaseDatas()
		{
			return this.GetPurchaseData(IAPTimePackData.PackType.Month);
		}

		public List<PurchaseCommonData.PurchaseData> GetPurchaseData(IAPTimePackData.PackType packType)
		{
			List<PurchaseCommonData.PurchaseData> list = new List<PurchaseCommonData.PurchaseData>();
			foreach (PurchaseCommonData.PurchaseData purchaseData in this.commonData.GetAllPurchaseData(IAPProductType.ShopTimePack))
			{
				if (purchaseData != null)
				{
					IAP_GiftPacks elementById = GameApp.Table.GetManager().GetIAP_GiftPacksModelInstance().GetElementById(purchaseData.m_id);
					if (elementById != null && elementById.packType == (int)packType && elementById.hide <= 0)
					{
						list.Add(purchaseData);
					}
				}
			}
			IAP_PurchaseModel model = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance();
			list.Sort((PurchaseCommonData.PurchaseData a, PurchaseCommonData.PurchaseData b) => model.GetElementById(a.m_id).priority.CompareTo(model.GetElementById(b.m_id).priority));
			return list;
		}

		public bool IsHaveRedPointForDaily()
		{
			return this.IsHaveRedPoint(IAPTimePackData.PackType.Daily);
		}

		public bool IsHaveRedPointForWeekly()
		{
			return this.IsHaveRedPoint(IAPTimePackData.PackType.Weekly);
		}

		public bool IsHaveRedPointForMonth()
		{
			return this.IsHaveRedPoint(IAPTimePackData.PackType.Month);
		}

		private bool IsHaveRedPoint(IAPTimePackData.PackType packType)
		{
			bool flag = false;
			if (this.GetCountDownByPackType(packType) <= 0L)
			{
				flag = true;
			}
			else
			{
				foreach (PurchaseCommonData.PurchaseData purchaseData in this.GetPurchaseData(packType))
				{
					if (purchaseData != null && this.IsShowRedPoint(purchaseData.m_id))
					{
						flag = true;
						break;
					}
				}
			}
			return flag;
		}

		public IAP_GiftPacks GetCurrentEnergyPack()
		{
			List<IAP_GiftPacks> list = new List<IAP_GiftPacks>();
			IList<IAP_GiftPacks> allElements = GameApp.Table.GetManager().GetIAP_GiftPacksModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				if (allElements[i].group == 801)
				{
					list.Add(allElements[i]);
				}
			}
			list.Sort((IAP_GiftPacks a, IAP_GiftPacks b) => a.id.CompareTo(b.id));
			for (int j = 0; j < list.Count; j++)
			{
				IAP_GiftPacks iap_GiftPacks = list[j];
				int buyCount = this.GetBuyCount(iap_GiftPacks.id);
				IAP_Purchase elementById = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(iap_GiftPacks.id);
				if (elementById.limitCount > 0 && buyCount < elementById.limitCount)
				{
					return iap_GiftPacks;
				}
				if (elementById.limitCount == 0)
				{
					return iap_GiftPacks;
				}
			}
			return null;
		}

		private readonly PurchaseCommonData commonData;

		private MapField<uint, uint> counts;

		private long dailyTime;

		private long weeklyTime;

		private long monthTime;

		public enum PackType
		{
			Daily = 1,
			Weekly,
			Month
		}

		public enum PackGroup
		{
			EnergyPack = 801
		}
	}
}
