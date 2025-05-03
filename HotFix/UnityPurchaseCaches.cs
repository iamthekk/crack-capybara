using System;
using Framework.PurchaseManager;
using Proto.Pay;

namespace HotFix
{
	public class UnityPurchaseCaches : BasePurchaseCaches
	{
		protected override void OnCheckNext(Action finished, int index)
		{
			if (index >= this.m_data.m_datas.Count || this.m_data.m_datas.Count == 0)
			{
				this.m_isChecking = false;
				if (finished != null)
				{
					finished();
				}
				return;
			}
			ProductMessageData data = this.m_data.m_datas[index];
			if (data.m_purchaseID > 0)
			{
				NetworkUtils.Purchase.SendPayInAppPurchaseRequest(data.m_purchaseID, data.m_receipt, delegate(bool isOk, PayInAppPurchaseResponse res)
				{
					if (!isOk)
					{
						if (res.Code == 204)
						{
							this.Remove(data);
						}
					}
					else
					{
						this.Remove(data);
					}
					int num2 = index + 1;
					this.OnCheckNext(finished, num2);
				});
				return;
			}
			HLog.LogError(string.Format("[PurchaseManager]Order data correction purchaseID={0}, productID={1},receipt={2}", data.m_purchaseID, data.m_productID, data.m_receipt));
			int num = index + 1;
			this.OnCheckNext(finished, num);
		}
	}
}
