using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using LocalModels.Model;

namespace HotFix
{
	public class IAPDiamondsPackData
	{
		public IAPDiamondsPackData(PurchaseCommonData commonDataVal)
		{
			this.commonData = commonDataVal;
		}

		public List<PurchaseCommonData.PurchaseData> GetShowPurchaseData()
		{
			List<PurchaseCommonData.PurchaseData> list = new List<PurchaseCommonData.PurchaseData>();
			foreach (PurchaseCommonData.PurchaseData purchaseData in this.commonData.GetAllPurchaseData(IAPProductType.ShopDiamondPack))
			{
				if (purchaseData != null)
				{
					bool flag = this.IsUnLimitCount(purchaseData);
					IAP_DiamondPacks elementById = GameApp.Table.GetManager().GetIAP_DiamondPacksModelInstance().GetElementById(purchaseData.m_id);
					int num;
					if (!string.IsNullOrEmpty(elementById.parameters) && int.TryParse(elementById.parameters, out num))
					{
						flag &= !this.IsUnLimitCount(num);
					}
					if (flag)
					{
						list.Add(purchaseData);
					}
				}
			}
			IAP_PurchaseModel iapPurchaseModel = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance();
			list.Sort((PurchaseCommonData.PurchaseData a, PurchaseCommonData.PurchaseData b) => iapPurchaseModel.GetElementById(a.m_id).priority.CompareTo(iapPurchaseModel.GetElementById(b.m_id).priority));
			return list;
		}

		private bool IsUnLimitCount(int id)
		{
			PurchaseCommonData.PurchaseData purchaseDataByID = this.commonData.GetPurchaseDataByID(id);
			return this.IsUnLimitCount(purchaseDataByID);
		}

		private bool IsUnLimitCount(PurchaseCommonData.PurchaseData data)
		{
			if (data == null)
			{
				return false;
			}
			IAP_Purchase elementById = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(data.m_id);
			return elementById != null && (elementById.limitCount == 0 || (ulong)data.m_tolBuyCount < (ulong)((long)elementById.limitCount));
		}

		public bool IsShowRedPoint(int tableID)
		{
			IAP_Purchase elementById = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(tableID);
			return elementById != null && elementById.price1 == 0f && this.IsUnLimitCount(tableID);
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
