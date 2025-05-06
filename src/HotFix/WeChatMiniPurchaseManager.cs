using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Framework;
using Framework.PurchaseManager;
using Habby.CustomEvent;
using HabbySDK.WebGame;
using LocalModels.Bean;
using Proto.Pay;
using Shop.Arena;
using UnityEngine;
using UnityEngine.Networking;

namespace HotFix
{
	public class WeChatMiniPurchaseManager : IPurchaseManager
	{
		private void OnPurchaseClicked(int tableId, string productId, int extraType, string extraInfo, Action<bool, WeChatMiniPurchaseManager.PayPurchaseRespons> callback = null)
		{
			try
			{
				IAP_Purchase table = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(tableId);
				IAP_platformID platform = GameApp.Table.GetManager().GetIAP_platformIDModelInstance().GetElementById(table.platformID);
				this.SetProgress(true);
				LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
				long serverUTC = dataModule.ServerUTC;
				long preOrderId = serverUTC;
				GameApp.SDK.Analyze.Track_IAPOrder(GameTGAIAPParam.Create(tableId), preOrderId.ToString(), -1);
				Action<bool, WeChatMiniPurchaseManager.PayPurchaseRespons> <>9__1;
				this.SendPayPreOrderRequestFunc(productId, extraType, extraInfo, table, preOrderId, dataModule, delegate(bool success, WeChatMiniPurchaseManager.PayPurchaseRespons resp)
				{
					if (!success)
					{
						Action<bool, WeChatMiniPurchaseManager.PayPurchaseRespons> callback3 = callback;
						if (callback3 != null)
						{
							callback3(false, resp);
						}
						this.SetProgress(false);
						return;
					}
					this.putRecordPool(preOrderId, new WeChatMiniPurchaseManager.PayRecordData());
					WeChatMiniPurchaseManager <>4__this = this;
					string productId2 = productId;
					IAP_Purchase table2 = table;
					long preOrderId2 = preOrderId;
					LoginDataModule dataModule2 = dataModule;
					int extraType2 = extraType;
					string extraInfo2 = extraInfo;
					PayPreOrderResponse payPreOrderResponse = resp.payPreOrderResponse;
					Action<bool, WeChatMiniPurchaseManager.PayPurchaseRespons> action;
					if ((action = <>9__1) == null)
					{
						action = (<>9__1 = delegate(bool success, WeChatMiniPurchaseManager.PayPurchaseRespons res)
						{
							if (res.payInAppPurchaseResponse != null)
							{
								this.VerifyAllOrders();
							}
							if (!success)
							{
								this.changeInquireState(preOrderId);
								Action<bool, WeChatMiniPurchaseManager.PayPurchaseRespons> callback4 = callback;
								if (callback4 == null)
								{
									return;
								}
								callback4(false, res);
								return;
							}
							else
							{
								this.GameIapSuccessBroadcastFunc(dataModule, res, platform);
								this.deleteRecordPool(preOrderId);
								Action<bool, WeChatMiniPurchaseManager.PayPurchaseRespons> callback5 = callback;
								if (callback5 == null)
								{
									return;
								}
								callback5(true, res);
								return;
							}
						});
					}
					<>4__this.OderPuchaseFunc(productId2, table2, preOrderId2, dataModule2, extraType2, extraInfo2, payPreOrderResponse, action);
				});
			}
			catch (Exception ex)
			{
				Action<bool, WeChatMiniPurchaseManager.PayPurchaseRespons> callback2 = callback;
				if (callback2 != null)
				{
					callback2(false, new WeChatMiniPurchaseManager.PayPurchaseRespons
					{
						reason = string.Format("【OnPurchaseClicked】下单执行失败{0}", ex)
					});
				}
				throw;
			}
		}

		private void VerifyOrder(long orderNumber, Action<bool> callback)
		{
			NetworkUtils.MiniPurchase.SendPayInAppPurchaseRequest(orderNumber, 201, delegate(bool isOk, PayInAppPurchaseResponse resp)
			{
				if (isOk && resp.CommonData.Reward != null && resp.CommonData.Reward.Count > 0)
				{
					DxxTools.UI.OpenRewardCommon(resp.CommonData.Reward, null, true);
				}
				Action<bool> callback2 = callback;
				if (callback2 == null)
				{
					return;
				}
				callback2(isOk);
			}, true);
		}

		private void GameIapSuccessBroadcastFunc(LoginDataModule dataModule, WeChatMiniPurchaseManager.PayPurchaseRespons res, IAP_platformID platform)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>
			{
				{ "userid", dataModule.userId },
				{ "orderId", res.prorderId },
				{ "payType", "weixinpay" },
				{ "price", platform.CNprice },
				{ "productName", platform.productID }
			};
			HabbyEventDispatch.Send("event.game.iap.success", dictionary);
		}

		private async Task OderPuchaseFunc(string productId, IAP_Purchase table, long preOrderId, LoginDataModule dataModule, int extraType, string extraInfo, PayPreOrderResponse resp, Action<bool, WeChatMiniPurchaseManager.PayPurchaseRespons> callback = null)
		{
			DeviceSystem system = GameApp.SDK.WebGameAPI.GetSystem();
			IAP_platformID elementById = GameApp.Table.GetManager().GetIAP_platformIDModelInstance().GetElementById(table.platformID);
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(2, table.nameID);
			string text = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", new object[] { dataModule.userId, productId, extraType, extraInfo, preOrderId, infoByID });
			switch (system)
			{
			case 1:
			case 4:
				Debug.Log(string.Format("purchase WX IOS enter: {0}", GameApp.SDK.WebGameAPI.GetSystem()));
				GameApp.SDK.WebGameAPI.PurchaseInServiceTalk(elementById.CNprice.ToString(), dataModule.userId.ToString(), productId, infoByID, "201", preOrderId.ToString(), GameApp.SDK.WebGameConfig.wxMsgImageUrl, string.Empty, text, delegate(PayContent rsp)
				{
					base.<OderPuchaseFunc>g__CallbackFunc|2(rsp);
				});
				break;
			case 2:
			case 3:
			case 5:
				GameApp.SDK.WebGameAPI.PurchaseInGame(resp.WeChatMiniGameOrderDto.SignData, resp.WeChatMiniGameOrderDto.PaySig, resp.WeChatMiniGameOrderDto.Signature, preOrderId.ToString(), delegate(PayContent rsp)
				{
					base.<OderPuchaseFunc>g__CallbackFunc|2(rsp);
				});
				break;
			default:
			{
				Action<bool, WeChatMiniPurchaseManager.PayPurchaseRespons> callback2 = callback;
				if (callback2 != null)
				{
					callback2(false, new WeChatMiniPurchaseManager.PayPurchaseRespons
					{
						success = false,
						payInAppPurchaseResponse = null,
						productId = productId,
						prorderId = (int)preOrderId,
						reason = "平台未识别" + system.ToString()
					});
				}
				throw new NotImplementedException();
			}
			}
		}

		private async Task<string> GetLocalConfigPlatformFunc()
		{
			return (await UnityWebRequest.Get("https://capybara.habby.cn/cn_sdk_test/platform.index").SendWebRequest()).downloadHandler.text;
		}

		private async Task SendPayPreOrderRequestFunc(string productId, int extraType, string extraInfo, IAP_Purchase table, long _preOrderId, LoginDataModule dataModule, Action<bool, WeChatMiniPurchaseManager.PayPurchaseRespons> callback = null)
		{
			try
			{
				NetworkUtils.MiniPurchase.SendPayPreOrderRequest(table.id, productId, extraType, extraInfo, _preOrderId, delegate(bool isOk, PayPreOrderResponse response)
				{
					callback(isOk, new WeChatMiniPurchaseManager.PayPurchaseRespons
					{
						success = isOk,
						payInAppPurchaseResponse = new PayInAppPurchaseResponse(),
						payPreOrderResponse = response,
						productId = productId,
						prorderId = (int)_preOrderId,
						reason = ((ProtoNetWorkCodeName)response.Code).ToString()
					});
				});
			}
			catch (Exception ex)
			{
				HLog.LogError(string.Format("[Error][WeChatMiniPurchaseManager.click]_____{0}", ex));
				Action<bool, WeChatMiniPurchaseManager.PayPurchaseRespons> callback2 = callback;
				if (callback2 != null)
				{
					callback2(false, new WeChatMiniPurchaseManager.PayPurchaseRespons
					{
						success = false,
						payInAppPurchaseResponse = null,
						productId = productId,
						reason = "预下单失败" + ex.ToString()
					});
				}
				throw;
			}
		}

		public void OnInit()
		{
			this.PayRecord.Clear();
			this.m_priceDic.Clear();
			this.m_cn_priceDic.Clear();
			this.m_callBacks.Clear();
			try
			{
				IList<IAP_Purchase> allElements = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetAllElements();
				for (int i = 0; i < allElements.Count; i++)
				{
					IAP_Purchase iap_Purchase = allElements[i];
					IAP_platformID elementById = GameApp.Table.GetManager().GetIAP_platformIDModelInstance().GetElementById(iap_Purchase.platformID);
					if (iap_Purchase != null && elementById != null)
					{
						this.m_priceDic[elementById.productID] = iap_Purchase.price1;
						this.m_cn_priceDic[elementById.productID] = iap_Purchase.CNprice;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		public void OnDeInit()
		{
			this.m_priceDic.Clear();
		}

		public string GetPriceForProductID(string productID)
		{
			float num;
			this.m_cn_priceDic.TryGetValue(productID, out num);
			if (num > 0f)
			{
				return num.ToString();
			}
			Debug.LogWarning("GetPriceForProductID error: can not find productID: " + productID);
			return "0";
		}

		public string GetPriceForPlatformID(int platformID)
		{
			IAP_platformID elementById = GameApp.Table.GetManager().GetIAP_platformIDModelInstance().GetElementById(platformID);
			if (elementById != null)
			{
				return this.GetPriceForProductID(elementById.productID);
			}
			Debug.LogWarning("GetPriceForPlatformID error: can not find platformID: " + platformID.ToString());
			return string.Empty;
		}

		public string GetIsoCodeForProductID(string productID)
		{
			return "CNY";
		}

		public string GetIsoCodeForPlatformID(int platformID)
		{
			return "CNY";
		}

		public float GetLocalizedPriceForPlatformID(int platformID)
		{
			IAP_platformID elementById = GameApp.Table.GetManager().GetIAP_platformIDModelInstance().GetElementById(platformID);
			if (elementById != null)
			{
				return this.GetLocalizedPriceForProductID(elementById.productID);
			}
			Debug.LogWarning("GetLocalizedPriceForPlatformID error: can not find platformID: " + platformID.ToString());
			return -1f;
		}

		public float GetLocalizedPriceForProductID(string productID)
		{
			float num;
			this.m_cn_priceDic.TryGetValue(productID, out num);
			if (num <= 0f)
			{
				Debug.LogWarning("GetLocalizedPriceForProductID error: can not find productID: " + productID);
			}
			return num;
		}

		public void Buy(int purchaseId, int extraType, string extraInfo, Action<bool> isSuccess, Action onCloseRewardUI = null)
		{
			WeChatMiniPurchaseManager.<>c__DisplayClass14_0 CS$<>8__locals1 = new WeChatMiniPurchaseManager.<>c__DisplayClass14_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.isSuccess = isSuccess;
			CS$<>8__locals1.onCloseRewardUI = onCloseRewardUI;
			try
			{
				IAP_Purchase table = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(purchaseId);
				if (GameApp.Table.GetManager().GetIAP_platformIDModelInstance().GetElementById(table.platformID)
					.CNprice <= 0f)
				{
					NetworkUtils.Purchase.SendBuyShopFreeIAPItemRequest(purchaseId, extraType, extraInfo, delegate(bool isOk, ShopFreeIAPItemResponse response)
					{
						base.<Buy>g__OnBuyCallBack|2(isOk, isOk ? response.CommonData.Reward : null);
					});
				}
				else
				{
					this.OnPurchaseClicked(table.id, purchaseId.ToString(), extraType, extraInfo, delegate(bool isOk, WeChatMiniPurchaseManager.PayPurchaseRespons resp)
					{
						if (!isOk)
						{
							GameApp.SDK.Analyze.Track_IAPFail(GameTGAIAPParam.Create(table.id), resp.prorderId.ToString(), resp.reason, 1);
						}
						else
						{
							GameApp.SDK.Analyze.Track_IAPSuccess(GameTGAIAPParam.Create(table.id), resp.prorderId.ToString(), resp.IsSandBox, resp.payInAppPurchaseResponse.CommonData.Reward);
						}
						CS$<>8__locals1.<Buy>g__OnBuyCallBack|2(isOk, isOk ? ((resp != null) ? resp.payInAppPurchaseResponse.CommonData.Reward : null) : null);
					});
				}
			}
			catch (Exception ex)
			{
				IAP_Purchase elementById = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(purchaseId);
				GameApp.SDK.Analyze.Track_IAPFail(GameTGAIAPParam.Create(elementById.id), string.Format("支付中失败{0}", ex), -1);
			}
		}

		public bool RemovePurchaseLinkData(string productID, long timestamp)
		{
			throw new NotImplementedException();
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

		public void putRecordPool(long num, WeChatMiniPurchaseManager.PayRecordData data)
		{
			this.PayRecord.TryAdd(num, data);
		}

		public void deleteRecordPool(long num)
		{
			if (this.PayRecord.ContainsKey(num))
			{
				this.PayRecord.Remove(num);
			}
		}

		public void changeInquireState(long num)
		{
			if (this.PayRecord.ContainsKey(num))
			{
				this.PayRecord[num].IsInquire = false;
			}
		}

		public void VerifyAllOrders()
		{
			List<long> verifiedOrders = new List<long>();
			using (Dictionary<long, WeChatMiniPurchaseManager.PayRecordData>.KeyCollection.Enumerator enumerator = this.PayRecord.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					long orderNumber = enumerator.Current;
					if (!this.PayRecord[orderNumber].IsInquire)
					{
						this.VerifyOrder(orderNumber, delegate(bool isOk)
						{
							if (isOk)
							{
								verifiedOrders.Add(orderNumber);
							}
						});
					}
				}
			}
			foreach (long num in verifiedOrders)
			{
				this.deleteRecordPool(num);
			}
		}

		public static string GeneratePaySig(string appKey, string signData)
		{
			string text = "requestMidasPaymentGameItem&" + signData;
			byte[] bytes = Encoding.UTF8.GetBytes(appKey);
			byte[] bytes2 = Encoding.UTF8.GetBytes(text);
			string text2;
			using (HMACSHA256 hmacsha = new HMACSHA256(bytes))
			{
				byte[] array = hmacsha.ComputeHash(bytes2);
				StringBuilder stringBuilder = new StringBuilder();
				foreach (byte b in array)
				{
					stringBuilder.AppendFormat("{0:x2}", b);
				}
				text2 = stringBuilder.ToString();
			}
			return text2;
		}

		public Dictionary<string, float> m_priceDic = new Dictionary<string, float>();

		public Dictionary<string, float> m_cn_priceDic = new Dictionary<string, float>();

		public Dictionary<string, WeChatMiniPurchaseManager.PayData> m_callBacks = new Dictionary<string, WeChatMiniPurchaseManager.PayData>();

		public Dictionary<long, WeChatMiniPurchaseManager.PayRecordData> PayRecord = new Dictionary<long, WeChatMiniPurchaseManager.PayRecordData>();

		public class PayData
		{
			public string m_productID;

			public int m_extraType;

			public string m_extraInfo;

			public ulong m_preOrderID;

			public Action<bool, PayInAppPurchaseResponse> callback;
		}

		public class PayObj
		{
			public string activityId;

			public int source;

			public string group_id;
		}

		public class PayPreOrderRequest
		{
			public int channelId { get; set; }

			public string productId { get; set; }

			public string custom { get; set; }

			public string extraInfo { get; set; }

			public long preOrderId { get; set; }
		}

		public class PayMessage
		{
			public long UserId { get; set; }

			public WeChatMiniPurchaseManager.Attach Attach { get; set; }

			public string ProductId { get; set; }
		}

		public class Attach
		{
			public long userId { get; set; }

			public string productId { get; set; }

			public int extraType { get; set; }

			public string extraInfo { get; set; }

			public long preOrderId { get; set; }

			public override string ToString()
			{
				return string.Format("{0}|{1}|{2}|{3}|{4}", new object[] { this.userId, this.productId, this.extraType, this.extraInfo, this.preOrderId });
			}
		}

		[Serializable]
		public class SignData
		{
			public string attach;

			public int buyQuantity;

			public string currencyType;

			public int env;

			public int goodsPrice;

			public string mode;

			public string offerId;

			public string outTradeNo;

			public string productId;

			public string platform;

			public string zoneid;
		}

		public class PayPurchaseRespons
		{
			public int IsSandBox
			{
				get
				{
					return 0;
				}
			}

			public bool success;

			public int prorderId;

			public string productId = "";

			public string reason = "";

			public PayInAppPurchaseResponse payInAppPurchaseResponse;

			public PayPreOrderResponse payPreOrderResponse;
		}

		public class PayRecordData
		{
			public Action<List<WeChatMiniPurchaseManager.IRewardDto>> SuccessCb { get; set; }

			public Action<WeChatMiniPurchaseManager.HttpError> ErrorCb { get; set; }

			public bool IsInquire { get; set; }

			public PayRecordData()
			{
				this.IsInquire = true;
			}
		}

		public class IRewardDto
		{
			public long UserId { get; set; }
		}

		public class HttpError
		{
			public string failreason { get; set; }

			public string errcode { get; set; }
		}
	}
}
