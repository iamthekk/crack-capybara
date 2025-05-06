using System;

namespace Framework.PurchaseManager
{
	public interface IPurchaseManager
	{
		void OnInit();

		void OnDeInit();

		string GetPriceForProductID(string productID);

		string GetPriceForPlatformID(int platformID);

		string GetIsoCodeForProductID(string productID);

		string GetIsoCodeForPlatformID(int platformID);

		float GetLocalizedPriceForPlatformID(int platformID);

		float GetLocalizedPriceForProductID(string productID);

		void Buy(int purchaseId, int extraType, string extraInfo, Action<bool> isSuccess, Action onCloseRewardUI = null);

		bool RemovePurchaseLinkData(string productID, long timestamp);
	}
}
