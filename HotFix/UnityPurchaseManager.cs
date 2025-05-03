using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using Framework.Logic;
using Framework.PurchaseManager;
using LocalModels.Bean;
using Proto.Pay;
using Shop.Arena;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace HotFix
{
	public class UnityPurchaseManager : IPurchaseManager
	{
		internal UnityPurchaseManager.PayData GetPayDataByProductID(string productID)
		{
			if (this.m_callBacks != null)
			{
				List<UnityPurchaseManager.PayData> list = this.m_callBacks.Values.ToList<UnityPurchaseManager.PayData>();
				list.Sort((UnityPurchaseManager.PayData a, UnityPurchaseManager.PayData b) => b.sortId.CompareTo(a.sortId));
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].m_productID.Equals(productID))
					{
						return list[i];
					}
				}
			}
			return null;
		}

		public void OnInit()
		{
			this.m_listener = new UnityPurchaseManager.UnityStoreListener();
			this.m_listener.m_onInitializeFailed = new Action<InitializationFailureReason>(this.OnInitializeFailed);
			this.m_listener.m_onProcessPurchase = new Func<PurchaseEventArgs, PurchaseProcessingResult>(this.ProcessPurchase);
			this.m_listener.m_onPurchaseFailed = new Action<Product>(this.OnPurchaseFailed);
			this.m_listener.m_onInitialized = new Action<IStoreController, IExtensionProvider>(this.OnInitialized);
			this.ReloadPurchaseLinkData();
			StandardPurchasingModule standardPurchasingModule = StandardPurchasingModule.Instance();
			standardPurchasingModule.useFakeStoreUIMode = 1;
			ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(standardPurchasingModule, Array.Empty<IPurchasingModule>());
			configurationBuilder.useCatalogProvider = true;
			foreach (ProductCatalogItem productCatalogItem in ProductCatalog.LoadDefaultCatalog().allValidProducts)
			{
				if (productCatalogItem.allStoreIDs.Count > 0)
				{
					IDs ds = new IDs();
					foreach (StoreID storeID in productCatalogItem.allStoreIDs)
					{
						ds.Add(storeID.id, new string[] { storeID.store });
					}
					configurationBuilder.AddProduct(productCatalogItem.id, productCatalogItem.type, ds);
				}
				else
				{
					configurationBuilder.AddProduct(productCatalogItem.id, productCatalogItem.type);
				}
			}
			this.m_priceDic.Clear();
			IList<IAP_platformID> allElements = GameApp.Table.GetManager().GetIAP_platformIDModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				IAP_platformID iap_platformID = allElements[i];
				if (iap_platformID != null)
				{
					this.m_priceDic[iap_platformID.productID] = string.Format("{0}", iap_platformID.price);
				}
			}
			this.m_callBacks.Clear();
			UnityPurchasing.Initialize(this.m_listener, configurationBuilder);
		}

		public void OnDeInit()
		{
			this.m_priceDic.Clear();
			this.m_listener = null;
		}

		private bool GetProductDataForProductID(string productID, out Product product)
		{
			if (this.m_controller == null || this.m_controller.products == null)
			{
				product = null;
				return false;
			}
			product = this.m_controller.products.WithID(productID);
			return product != null;
		}

		public string GetPriceForProductID(string productID)
		{
			string localizedPriceString;
			this.m_priceDic.TryGetValue(productID, out localizedPriceString);
			if (string.IsNullOrEmpty(productID))
			{
				return localizedPriceString;
			}
			Product product;
			if (!this.GetProductDataForProductID(productID, out product) || product.metadata == null)
			{
				return localizedPriceString;
			}
			localizedPriceString = product.metadata.localizedPriceString;
			return localizedPriceString;
		}

		public string GetPriceForPlatformID(int platformID)
		{
			IAP_platformID elementById = GameApp.Table.GetManager().GetIAP_platformIDModelInstance().GetElementById(platformID);
			if (elementById != null)
			{
				return this.GetPriceForProductID(elementById.productID);
			}
			return "";
		}

		public string GetIsoCodeForProductID(string productID)
		{
			string empty = string.Empty;
			if (string.IsNullOrEmpty(productID))
			{
				return empty;
			}
			Product product;
			if (!this.GetProductDataForProductID(productID, out product) || product.metadata == null)
			{
				return empty;
			}
			return product.metadata.isoCurrencyCode;
		}

		public string GetIsoCodeForPlatformID(int platformID)
		{
			IAP_platformID elementById = GameApp.Table.GetManager().GetIAP_platformIDModelInstance().GetElementById(platformID);
			if (elementById != null)
			{
				return this.GetIsoCodeForProductID(elementById.productID);
			}
			return "";
		}

		public float GetLocalizedPriceForPlatformID(int platformID)
		{
			IAP_platformID elementById = GameApp.Table.GetManager().GetIAP_platformIDModelInstance().GetElementById(platformID);
			if (elementById != null)
			{
				return this.GetLocalizedPriceForProductID(elementById.productID);
			}
			return -1f;
		}

		public float GetLocalizedPriceForProductID(string productID)
		{
			float num = -1f;
			if (string.IsNullOrEmpty(productID))
			{
				return num;
			}
			Product product;
			if (!this.GetProductDataForProductID(productID, out product) || product.metadata == null)
			{
				return num;
			}
			return (float)product.metadata.localizedPrice;
		}

		public void Buy(int purchaseId, int extraType, string extraInfo, Action<bool> isSuccess, Action onCloseRewardUI = null)
		{
			IAP_Purchase elementById = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(purchaseId);
			if (elementById == null)
			{
				return;
			}
			if (elementById.price1 <= 0f)
			{
				NetworkUtils.Purchase.SendBuyShopFreeIAPItemRequest(purchaseId, extraType, extraInfo, delegate(bool isOk, ShopFreeIAPItemResponse response)
				{
					base.<Buy>g__OnBuyCallBack|2(isOk, isOk ? response.CommonData.Reward : null);
				});
				return;
			}
			if (elementById.platformID == 0)
			{
				return;
			}
			IAP_platformID elementById2 = GameApp.Table.GetManager().GetIAP_platformIDModelInstance().GetElementById(elementById.platformID);
			if (elementById2 == null)
			{
				return;
			}
			this.OnPurchaseClicked(elementById.id, elementById2.productID, extraType, extraInfo, delegate(bool isOk, PayInAppPurchaseResponse resp)
			{
				base.<Buy>g__OnBuyCallBack|2(isOk, isOk ? resp.CommonData.Reward : null);
			});
		}

		public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
		{
			this.m_controller = controller;
			this.m_appleExtensions = extensions.GetExtension<IAppleExtensions>();
			this.m_appleExtensions.RegisterPurchaseDeferredListener(new Action<Product>(this.OnDeferred));
		}

		public void OnInitializeFailed(InitializationFailureReason error)
		{
		}

		public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
		{
			string id = purchaseEvent.purchasedProduct.definition.id;
			string receipt = purchaseEvent.purchasedProduct.receipt;
			UnityPurchaseManager.PayData payData = this.GetPayDataByProductID(id);
			int purchaseID = ((payData != null) ? payData.m_purchaseID : 0);
			PurchaseLinkData purchaseLinkData = ((payData != null) ? this.GetPurchaseLinkData(payData.m_purchaseID, payData.m_productID, payData.m_timestamp) : null);
			if (payData == null)
			{
				purchaseLinkData = this.GetPurchaseLinkData(id);
			}
			ProductMessageData data = new ProductMessageData();
			data.m_productID = id;
			data.m_receipt = receipt;
			if (purchaseLinkData != null)
			{
				data.m_purchaseID = purchaseLinkData.m_purchaseID;
				data.m_timestamp = purchaseLinkData.m_timestamp;
			}
			GameApp.Purchase.Caches.Add(data);
			if (payData != null && payData.callback != null)
			{
				GameApp.SDK.Analyze.Track_IAPFinishChannel(GameTGAIAPParam.Create(payData.m_purchaseID), GameTGATools.Ins.PreOrderIDToOrderID((int)payData.m_preOrderID));
				NetworkUtils.Purchase.SendPayInAppPurchaseRequest(purchaseID, receipt, payData.m_extraType, payData.m_extraInfo, payData.m_preOrderID, delegate(bool isOk, PayInAppPurchaseResponse res)
				{
					this.SetProgress(false);
					if (!isOk)
					{
						GameApp.SDK.Analyze.Track_IAPFail(GameTGAIAPParam.Create(payData.m_purchaseID), GameTGATools.Ins.PreOrderIDToOrderID((int)payData.m_preOrderID), string.Format("Server Error {0}", res.Code), res.IsSandBox ? 1 : 0);
						return;
					}
					GameApp.Purchase.Caches.Remove(data);
					this.RemovePurchaseLinkData(payData.m_purchaseID, payData.m_productID, payData.m_timestamp);
					GameApp.Data.GetDataModule(DataName.IAPDataModule).UpdateTGAInfo(res.TgaInfoDto);
					if (payData.callback != null)
					{
						payData.callback(isOk, res);
					}
					GameAFTools.Ins.OnPaySuccess(payData.m_purchaseID);
					GameApp.SDK.Analyze.Track_IAPSuccess(GameTGAIAPParam.Create(payData.m_purchaseID), GameTGATools.Ins.PreOrderIDToOrderID((int)payData.m_preOrderID), res.IsSandBox ? 1 : 0, res.CommonData.Reward);
					GameApp.SDK.AppsFlyerSDK.Track_AFPurchase(purchaseEvent.purchasedProduct.metadata.localizedPrice, purchaseEvent.purchasedProduct.metadata.isoCurrencyCode, purchaseEvent.purchasedProduct.definition.id);
					this.m_callBacks.Remove(purchaseID);
				});
			}
			return 0;
		}

		public void OnPurchaseFailed(Product product)
		{
			this.SetProgress(false);
			if (product == null)
			{
				return;
			}
			if (product.definition == null)
			{
				return;
			}
			UnityPurchaseManager.PayData payDataByProductID = this.GetPayDataByProductID(product.definition.id);
			int num = ((payDataByProductID != null) ? payDataByProductID.m_purchaseID : 0);
			if (payDataByProductID == null)
			{
				return;
			}
			if (payDataByProductID.callback != null)
			{
				payDataByProductID.callback(false, null);
			}
			this.RemovePurchaseLinkData(payDataByProductID.m_purchaseID, payDataByProductID.m_productID, payDataByProductID.m_timestamp);
			this.m_callBacks.Remove(num);
		}

		private void OnDeferred(Product product)
		{
			this.SetProgress(false);
		}

		private void OnPurchaseClicked(int purchaseID, string productID, int extraType, string extraInfo, Action<bool, PayInAppPurchaseResponse> callback = null)
		{
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			long serverTime = dataModule.ServerUTC;
			UnityPurchaseManager.PayData data = new UnityPurchaseManager.PayData();
			data.m_purchaseID = purchaseID;
			data.m_productID = productID;
			data.m_extraType = extraType;
			data.m_extraInfo = extraInfo;
			data.callback = callback;
			data.sortId = Time.realtimeSinceStartup;
			data.m_timestamp = serverTime;
			this.m_callBacks[purchaseID] = data;
			NetworkUtils.Purchase.SendPayPreOrderRequest(purchaseID, productID, extraType, extraInfo, serverTime, delegate(bool isOk, PayPreOrderResponse response)
			{
				if (!isOk)
				{
					return;
				}
				data.m_preOrderID = response.PreOrderId;
				GameApp.SDK.Analyze.Track_IAPOrder(GameTGAIAPParam.Create(purchaseID), GameTGATools.Ins.PreOrderIDToOrderID((int)data.m_preOrderID));
				this.AddPurchaseLinkData(purchaseID, productID, serverTime);
				this.m_controller.InitiatePurchase(productID);
				this.SetProgress(true);
			});
		}

		protected void SetProgress(bool value)
		{
			if (value)
			{
				GameApp.View.ShowNetLoading(true);
				return;
			}
			GameApp.View.ShowNetLoading(false);
		}

		private void ReloadPurchaseLinkData()
		{
			this.purchaseOrderLinkDatas.Clear();
			string @string = Utility.PlayerPrefs.GetString("PUCHASELINKCACHE_Key", "");
			if (string.IsNullOrEmpty(@string))
			{
				return;
			}
			string[] array = @string.Split('|', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[0].Split(',', StringSplitOptions.None);
				try
				{
					PurchaseLinkData purchaseLinkData = new PurchaseLinkData();
					int.TryParse(array2[0], out purchaseLinkData.m_purchaseID);
					purchaseLinkData.m_productID = array2[1];
					long.TryParse(array2[2], out purchaseLinkData.m_timestamp);
					if (purchaseLinkData.m_purchaseID > 0 && !string.IsNullOrEmpty(purchaseLinkData.m_productID) && purchaseLinkData.m_timestamp > 0L)
					{
						this.purchaseOrderLinkDatas.Add(purchaseLinkData);
					}
				}
				catch (Exception ex)
				{
					HLog.LogException(ex);
				}
			}
		}

		private PurchaseLinkData GetPurchaseLinkData(string productID)
		{
			for (int i = this.purchaseOrderLinkDatas.Count - 1; i >= 0; i--)
			{
				PurchaseLinkData purchaseLinkData = this.purchaseOrderLinkDatas[i];
				if (purchaseLinkData.m_productID == productID)
				{
					return purchaseLinkData;
				}
			}
			return null;
		}

		private PurchaseLinkData GetPurchaseLinkData(int purchaseID, string productID, long timestamp)
		{
			for (int i = this.purchaseOrderLinkDatas.Count - 1; i >= 0; i--)
			{
				PurchaseLinkData purchaseLinkData = this.purchaseOrderLinkDatas[i];
				if (purchaseLinkData.m_purchaseID == purchaseID && purchaseLinkData.m_productID == productID && purchaseLinkData.m_timestamp == timestamp)
				{
					return purchaseLinkData;
				}
			}
			return null;
		}

		private void AddPurchaseLinkData(int purchaseID, string productID, long timestamp)
		{
			PurchaseLinkData purchaseLinkData = new PurchaseLinkData();
			purchaseLinkData.m_purchaseID = purchaseID;
			purchaseLinkData.m_productID = productID;
			purchaseLinkData.m_timestamp = timestamp;
			this.purchaseOrderLinkDatas.Add(purchaseLinkData);
			if (this.purchaseOrderLinkDatas.Count > 100)
			{
				this.purchaseOrderLinkDatas.RemoveAt(0);
			}
			this.SavePurchaseLinkData();
		}

		public bool RemovePurchaseLinkData(string productID, long timestamp)
		{
			bool flag = false;
			for (int i = this.purchaseOrderLinkDatas.Count - 1; i >= 0; i--)
			{
				PurchaseLinkData purchaseLinkData = this.purchaseOrderLinkDatas[i];
				if (purchaseLinkData.m_productID.Equals(productID) && purchaseLinkData.m_timestamp == timestamp)
				{
					this.purchaseOrderLinkDatas.RemoveAt(i);
					flag = true;
					break;
				}
			}
			if (flag)
			{
				this.SavePurchaseLinkData();
			}
			return flag;
		}

		private bool RemovePurchaseLinkData(int purchaseID, string productID, long timestamp)
		{
			bool flag = false;
			for (int i = this.purchaseOrderLinkDatas.Count - 1; i >= 0; i--)
			{
				PurchaseLinkData purchaseLinkData = this.purchaseOrderLinkDatas[i];
				if (purchaseLinkData.m_purchaseID == purchaseID && purchaseLinkData.m_productID.Equals(productID) && purchaseLinkData.m_timestamp == timestamp)
				{
					this.purchaseOrderLinkDatas.RemoveAt(i);
					flag = true;
					break;
				}
			}
			if (flag)
			{
				this.SavePurchaseLinkData();
			}
			return flag;
		}

		private void SavePurchaseLinkData()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.purchaseOrderLinkDatas.Count; i++)
			{
				PurchaseLinkData purchaseLinkData = this.purchaseOrderLinkDatas[i];
				if (i == 0)
				{
					stringBuilder.Append(string.Format("{0},{1},{2}", purchaseLinkData.m_purchaseID, purchaseLinkData.m_productID, purchaseLinkData.m_timestamp));
				}
				else
				{
					stringBuilder.Append(string.Format("|{0},{1},{2}", purchaseLinkData.m_purchaseID, purchaseLinkData.m_productID, purchaseLinkData.m_timestamp));
				}
			}
			Utility.PlayerPrefs.SetString("PUCHASELINKCACHE_Key", stringBuilder.ToString());
			Utility.PlayerPrefs.Save();
		}

		public Dictionary<int, UnityPurchaseManager.PayData> m_callBacks = new Dictionary<int, UnityPurchaseManager.PayData>();

		private UnityPurchaseManager.UnityStoreListener m_listener;

		private IStoreController m_controller;

		private IAppleExtensions m_appleExtensions;

		public Dictionary<string, string> m_priceDic = new Dictionary<string, string>();

		private List<PurchaseLinkData> purchaseOrderLinkDatas = new List<PurchaseLinkData>();

		public class PayData
		{
			public int m_purchaseID;

			public string m_productID;

			public int m_extraType;

			public string m_extraInfo;

			public ulong m_preOrderID;

			public long m_timestamp;

			public float sortId;

			public Action<bool, PayInAppPurchaseResponse> callback;
		}

		public class UnityStoreListener : IDetailedStoreListener, IStoreListener
		{
			public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
			{
				if (this.m_onInitialized != null)
				{
					this.m_onInitialized(controller, extensions);
				}
			}

			public void OnInitializeFailed(InitializationFailureReason error)
			{
				if (this.m_onInitializeFailed != null)
				{
					this.m_onInitializeFailed(error);
				}
			}

			public void OnInitializeFailed(InitializationFailureReason error, string message)
			{
				if (this.m_onInitializeFailed != null)
				{
					this.m_onInitializeFailed(error);
				}
			}

			public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
			{
				if (this.m_onProcessPurchase != null)
				{
					return this.m_onProcessPurchase(purchaseEvent);
				}
				return 0;
			}

			public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
			{
				if (product != null && product.definition != null)
				{
					UnityPurchaseManager unityPurchaseManager = GameApp.Purchase.Manager as UnityPurchaseManager;
					if (unityPurchaseManager != null)
					{
						UnityPurchaseManager.PayData payDataByProductID = unityPurchaseManager.GetPayDataByProductID(product.definition.id);
						if (payDataByProductID != null)
						{
							int purchaseID = payDataByProductID.m_purchaseID;
						}
						if (payDataByProductID != null)
						{
							unityPurchaseManager.RemovePurchaseLinkData(payDataByProductID.m_purchaseID, payDataByProductID.m_productID, payDataByProductID.m_timestamp);
							GameApp.SDK.Analyze.Track_IAPFail(GameTGAIAPParam.Create(payDataByProductID.m_purchaseID), GameTGATools.Ins.PreOrderIDToOrderID((int)payDataByProductID.m_preOrderID), failureReason.ToString(), -1);
						}
					}
				}
				if (this.m_onPurchaseFailed != null)
				{
					this.m_onPurchaseFailed(product);
				}
			}

			public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
			{
				if (product != null && product.definition != null)
				{
					UnityPurchaseManager unityPurchaseManager = GameApp.Purchase.Manager as UnityPurchaseManager;
					if (unityPurchaseManager != null)
					{
						UnityPurchaseManager.PayData payDataByProductID = unityPurchaseManager.GetPayDataByProductID(product.definition.id);
						if (payDataByProductID != null)
						{
							int purchaseID = payDataByProductID.m_purchaseID;
						}
						if (payDataByProductID != null)
						{
							unityPurchaseManager.RemovePurchaseLinkData(payDataByProductID.m_purchaseID, payDataByProductID.m_productID, payDataByProductID.m_timestamp);
							GameApp.SDK.Analyze.Track_IAPFail(GameTGAIAPParam.Create(payDataByProductID.m_purchaseID), GameTGATools.Ins.PreOrderIDToOrderID((int)payDataByProductID.m_preOrderID), failureDescription.reason.ToString() + "||" + failureDescription.message, -1);
						}
					}
				}
				if (this.m_onPurchaseFailed != null)
				{
					this.m_onPurchaseFailed(product);
				}
			}

			public Action<IStoreController, IExtensionProvider> m_onInitialized;

			public Action<InitializationFailureReason> m_onInitializeFailed;

			public Func<PurchaseEventArgs, PurchaseProcessingResult> m_onProcessPurchase;

			public Action<Product> m_onPurchaseFailed;
		}
	}
}
