using System;
using System.Collections.Generic;
using Framework;
using Google.Protobuf.Collections;
using LocalModels.Bean;

namespace HotFix
{
	public class PurchaseCommonData
	{
		public PurchaseCommonData.PurchaseData GetPurchaseDataByID(int id)
		{
			PurchaseCommonData.PurchaseData purchaseData;
			this.m_allPurchases.TryGetValue(id, out purchaseData);
			return purchaseData;
		}

		public void SetPurchaseData(MapField<uint, uint> buyIds, bool isClear)
		{
			if (isClear)
			{
				this.m_allPurchases.Clear();
			}
			foreach (IAP_Purchase iap_Purchase in GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetAllElements())
			{
				PurchaseCommonData.PurchaseData purchaseData;
				if (!this.m_allPurchases.TryGetValue(iap_Purchase.id, out purchaseData))
				{
					purchaseData = new PurchaseCommonData.PurchaseData();
					purchaseData.m_id = iap_Purchase.id;
					this.m_allPurchases[iap_Purchase.id] = purchaseData;
				}
				uint num;
				if (buyIds.TryGetValue((uint)iap_Purchase.id, ref num))
				{
					purchaseData.m_tolBuyCount = num;
				}
			}
		}

		public List<PurchaseCommonData.PurchaseData> GetAllPurchaseData(IAPProductType iapProductType)
		{
			List<PurchaseCommonData.PurchaseData> list = new List<PurchaseCommonData.PurchaseData>();
			foreach (KeyValuePair<int, PurchaseCommonData.PurchaseData> keyValuePair in this.m_allPurchases)
			{
				if (keyValuePair.Value != null && GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(keyValuePair.Key)
					.productType == (int)iapProductType)
				{
					list.Add(keyValuePair.Value);
				}
			}
			return list;
		}

		public List<PurchaseCommonData.PurchaseData> GetAllPurchaseDataByViewType(IAPViewType iapViewType)
		{
			List<PurchaseCommonData.PurchaseData> list = new List<PurchaseCommonData.PurchaseData>();
			foreach (KeyValuePair<int, PurchaseCommonData.PurchaseData> keyValuePair in this.m_allPurchases)
			{
				if (keyValuePair.Value != null && GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(keyValuePair.Key)
					.viewType == (int)iapViewType)
				{
					list.Add(keyValuePair.Value);
				}
			}
			return list;
		}

		public bool IsLimitCount(int id)
		{
			IAP_Purchase elementById = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(id);
			if (elementById == null)
			{
				return false;
			}
			PurchaseCommonData.PurchaseData purchaseData;
			this.m_allPurchases.TryGetValue(id, out purchaseData);
			return purchaseData != null && elementById.limitCount != 0 && (ulong)purchaseData.m_tolBuyCount >= (ulong)((long)elementById.limitCount);
		}

		private Dictionary<int, PurchaseCommonData.PurchaseData> m_allPurchases = new Dictionary<int, PurchaseCommonData.PurchaseData>();

		[RuntimeDefaultSerializedProperty]
		public class PurchaseData
		{
			public int m_id;

			public uint m_tolBuyCount;
		}
	}
}
