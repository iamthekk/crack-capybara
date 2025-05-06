
public class MiniPurchase
{
    public static void SendPayInAppPurchaseRequest(long preOrderID, int channelId, Action<bool, PayInAppPurchaseResponse> callback, bool ordercheck = false)
    {
        NetworkUtils.MiniPurchase.SendPayInAppPurchaseRequestInternal(preOrderID, channelId, 0, callback, ordercheck);
    }

    private static void SendPayInAppPurchaseRequestInternal(long preOrderID, int channelId, int attemptsLeft, Action<bool, PayInAppPurchaseResponse> callback, bool ordercheck = false)
    {
        PayInAppPurchaseRequest payInAppPurchaseRequest = new PayInAppPurchaseRequest();
        payInAppPurchaseRequest.PreOrderId = (ulong)preOrderID;
        payInAppPurchaseRequest.ChannelId = (uint)channelId;
        payInAppPurchaseRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(payInAppPurchaseRequest, delegate (IMessage response)
        {
            PayInAppPurchaseResponse payInAppPurchaseResponse = response as PayInAppPurchaseResponse;
            if (payInAppPurchaseResponse != null && payInAppPurchaseResponse.Code == 0)
            {
                EventArgsRefreshIAPCommonData instance = Singleton<EventArgsRefreshIAPCommonData>.Instance;
                instance.SetData(payInAppPurchaseResponse.RechargeIds, false);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshCommonData, instance);
                EventArgsRefreshIAPInfoData instance2 = Singleton<EventArgsRefreshIAPInfoData>.Instance;
                instance2.SetData(payInAppPurchaseResponse.IapInfo);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshIAPInfoData, instance2);
                Action<bool, PayInAppPurchaseResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, payInAppPurchaseResponse);
                return;
            }
            else
            {
                if (attemptsLeft > 1)
                {
                    NetworkUtils.MiniPurchase.SendPayInAppPurchaseRequestInternal(preOrderID, channelId, attemptsLeft - 1, callback, false);
                    return;
                }
                Action<bool, PayInAppPurchaseResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, payInAppPurchaseResponse);
                return;
            }
        }, !ordercheck, ordercheck, string.Empty, !ordercheck);
    }

    public static void SendPayPreOrderRequest(int purchaseId, string productId, int extraType, string extraInfo, long serverTime, Action<bool, PayPreOrderResponse> callback)
    {
        PayPreOrderRequest payPreOrderRequest = new PayPreOrderRequest();
        payPreOrderRequest.CommonParams = NetworkUtils.GetCommonParams();
        payPreOrderRequest.ProductId = purchaseId.ToString();
        payPreOrderRequest.ExtraInfo = extraInfo;
        payPreOrderRequest.PreOrderId = (ulong)serverTime;
        payPreOrderRequest.ExtraType = (uint)extraType;
        DeviceSystem system = GameApp.SDK.WebGameAPI.GetSystem();
        if (system - 2 <= 1 || system == 5)
        {
            payPreOrderRequest.ChannelId = 200U;
        }
        GameApp.NetWork.Send(payPreOrderRequest, delegate (IMessage response)
        {
            PayPreOrderResponse payPreOrderResponse = response as PayPreOrderResponse;
            if (payPreOrderResponse != null && payPreOrderResponse.Code == 0)
            {
                Action<bool, PayPreOrderResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, payPreOrderResponse);
                return;
            }
            else
            {
                if (payPreOrderResponse != null)
                {
                    int code = payPreOrderResponse.Code;
                }
                Action<bool, PayPreOrderResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, payPreOrderResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }
}
