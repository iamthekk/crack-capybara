using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using LocalModels.Model;

namespace HotFix
{
	public class IAPSuperValuePack
	{
		public IAPSuperValuePack(PurchaseCommonData commonDataVal)
		{
			this.commonData = commonDataVal;
		}

		public List<PurchaseCommonData.PurchaseData> GetShowPurchaseData()
		{
			List<PurchaseCommonData.PurchaseData> list = new List<PurchaseCommonData.PurchaseData>();
			foreach (PurchaseCommonData.PurchaseData purchaseData in this.commonData.GetAllPurchaseDataByViewType(IAPViewType.SuperValuePack))
			{
				if (purchaseData != null)
				{
					IAP_GiftPacks elementById = GameApp.Table.GetManager().GetIAP_GiftPacksModelInstance().GetElementById(purchaseData.m_id);
					if (elementById != null && elementById.hide == 0)
					{
						list.Add(purchaseData);
					}
				}
			}
			IAP_PurchaseModel iapPurchaseModel = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance();
			list.Sort((PurchaseCommonData.PurchaseData a, PurchaseCommonData.PurchaseData b) => iapPurchaseModel.GetElementById(a.m_id).priority.CompareTo(iapPurchaseModel.GetElementById(b.m_id).priority));
			return list;
		}

		public bool IsMaxBuyCount(int id)
		{
			int buyCount = this.GetBuyCount(id);
			IAP_Purchase elementById = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(id);
			return elementById != null && elementById.limitCount != 0 && buyCount >= elementById.limitCount;
		}

		public int GetBuyCount(int id)
		{
			PurchaseCommonData.PurchaseData purchaseDataByID = this.commonData.GetPurchaseDataByID(id);
			if (purchaseDataByID != null)
			{
				return (int)purchaseDataByID.m_tolBuyCount;
			}
			return 0;
		}

		public bool IsShowRedPoint(int tableID)
		{
			IAP_Purchase elementById = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(tableID);
			return elementById != null && elementById.price1 == 0f && !this.IsMaxBuyCount(tableID);
		}

		public bool IsHaveRedPoint()
		{
			bool flag = false;
			foreach (PurchaseCommonData.PurchaseData purchaseData in this.GetShowPurchaseData())
			{
				if (purchaseData != null && this.IsShowRedPoint(purchaseData.m_id))
				{
					flag = true;
					break;
				}
			}
			return flag;
		}

		private readonly PurchaseCommonData commonData;
	}
}
