
public class Purchase
{
    public static void SendPayInAppPurchaseRequest(int purchaseId, string receipt, int extraType, string extraInfo, ulong preOrderID, Action<bool, PayInAppPurchaseResponse> callback)
    {
        PayInAppPurchaseRequest payInAppPurchaseRequest = new PayInAppPurchaseRequest();
        payInAppPurchaseRequest.CommonParams = NetworkUtils.GetCommonParams();
        payInAppPurchaseRequest.ProductId = purchaseId.ToString();
        payInAppPurchaseRequest.ExtraInfo = extraInfo;
        payInAppPurchaseRequest.ReceiptData = receipt;
        payInAppPurchaseRequest.PreOrderId = preOrderID;
        payInAppPurchaseRequest.ExtraType = (uint)extraType;
        payInAppPurchaseRequest.PlatformIndex = (uint)Singleton<PlatformHelper>.Instance.GetPlatformIndex();
        payInAppPurchaseRequest.ChannelId = (uint)Singleton<PlatformHelper>.Instance.GetChannelIndex();
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
                Action<bool, PayInAppPurchaseResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, payInAppPurchaseResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void SendPayInAppPurchaseRequest(int purchaseId, string receipt, Action<bool, PayInAppPurchaseResponse> callback)
    {
        PayInAppPurchaseRequest payInAppPurchaseRequest = new PayInAppPurchaseRequest();
        payInAppPurchaseRequest.CommonParams = NetworkUtils.GetCommonParams();
        payInAppPurchaseRequest.ProductId = purchaseId.ToString();
        payInAppPurchaseRequest.ReceiptData = receipt;
        payInAppPurchaseRequest.PlatformIndex = 1U;
        payInAppPurchaseRequest.ChannelId = 2U;
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
                Action<bool, PayInAppPurchaseResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, payInAppPurchaseResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void SendPayPreOrderRequest(int purchaseId, string productId, int extraType, string extraInfo, long serverTime, Action<bool, PayPreOrderResponse> callback)
    {
        PayPreOrderRequest payPreOrderRequest = new PayPreOrderRequest();
        payPreOrderRequest.CommonParams = NetworkUtils.GetCommonParams();
        payPreOrderRequest.ProductId = purchaseId.ToString();
        payPreOrderRequest.ExtraInfo = extraInfo;
        payPreOrderRequest.PreOrderId = (ulong)serverTime;
        payPreOrderRequest.ExtraType = (uint)extraType;
        GameApp.NetWork.Send(payPreOrderRequest, delegate (IMessage response)
        {
            PayPreOrderResponse payPreOrderResponse = response as PayPreOrderResponse;
            if (payPreOrderResponse == null || payPreOrderResponse.Code != 0)
            {
                Action<bool, PayPreOrderResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(false, payPreOrderResponse);
                }
                if (payPreOrderResponse != null)
                {
                    int code = payPreOrderResponse.Code;
                }
                return;
            }
            Action<bool, PayPreOrderResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(true, payPreOrderResponse);
        }, true, false, string.Empty, true);
    }

    public static void BattlePassRewardRequest(List<uint> idlist, Action<bool, BattlePassRewardResponse> callback)
    {
        BattlePassRewardRequest battlePassRewardRequest = new BattlePassRewardRequest();
        battlePassRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
        battlePassRewardRequest.BattlePassRewardIdList.AddRange(idlist);
        GameApp.NetWork.Send(battlePassRewardRequest, delegate (IMessage response)
        {
            BattlePassRewardResponse battlePassRewardResponse = response as BattlePassRewardResponse;
            if (battlePassRewardResponse != null && battlePassRewardResponse.BattlePassDto != null)
            {
                EventArgsRefreshIAPBattlePassData instance = Singleton<EventArgsRefreshIAPBattlePassData>.Instance;
                instance.SetData(battlePassRewardResponse.BattlePassDto);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshIAPBattlePassData, instance);
            }
            if (battlePassRewardResponse != null && battlePassRewardResponse.Code == 0)
            {
                Action<bool, BattlePassRewardResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, battlePassRewardResponse);
                return;
            }
            else
            {
                Action<bool, BattlePassRewardResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, battlePassRewardResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void BattlePassBuyScoreRequest(uint addscore, Action<bool, BattlePassChangeScoreResponse> callback)
    {
        BattlePassChangeScoreRequest battlePassChangeScoreRequest = new BattlePassChangeScoreRequest();
        battlePassChangeScoreRequest.CommonParams = NetworkUtils.GetCommonParams();
        battlePassChangeScoreRequest.AddScore = addscore;
        GameApp.NetWork.Send(battlePassChangeScoreRequest, delegate (IMessage response)
        {
            BattlePassChangeScoreResponse battlePassChangeScoreResponse = response as BattlePassChangeScoreResponse;
            if (battlePassChangeScoreResponse != null && battlePassChangeScoreResponse.Code == 0)
            {
                EventArgsRefreshIAPBattlePassData instance = Singleton<EventArgsRefreshIAPBattlePassData>.Instance;
                instance.SetData(battlePassChangeScoreResponse.BattlePassDto);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshIAPBattlePassData, instance);
                Action<bool, BattlePassChangeScoreResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, battlePassChangeScoreResponse);
                return;
            }
            else
            {
                Action<bool, BattlePassChangeScoreResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, battlePassChangeScoreResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void BattlePassFinalRewardRequest(Action<bool, BattlePassFinalRewardResponse> callback)
    {
        BattlePassFinalRewardRequest battlePassFinalRewardRequest = new BattlePassFinalRewardRequest();
        battlePassFinalRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(battlePassFinalRewardRequest, delegate (IMessage response)
        {
            BattlePassFinalRewardResponse battlePassFinalRewardResponse = response as BattlePassFinalRewardResponse;
            if (battlePassFinalRewardResponse != null && battlePassFinalRewardResponse.Code == 0)
            {
                EventArgsRefreshIAPBattlePassData instance = Singleton<EventArgsRefreshIAPBattlePassData>.Instance;
                instance.SetData(battlePassFinalRewardResponse.BattlePassDto);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshIAPBattlePassData, instance);
                Action<bool, BattlePassFinalRewardResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, battlePassFinalRewardResponse);
                return;
            }
            else
            {
                Action<bool, BattlePassFinalRewardResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, battlePassFinalRewardResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void LevelFundRewardRequest(List<int> ids, Action<bool, LevelFundRewardResponse> callback)
    {
        LevelFundRewardRequest levelFundRewardRequest = new LevelFundRewardRequest();
        levelFundRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
        levelFundRewardRequest.LevelFundRewardId.AddRange(ids);
        GameApp.NetWork.Send(levelFundRewardRequest, delegate (IMessage response)
        {
            LevelFundRewardResponse levelFundRewardResponse = response as LevelFundRewardResponse;
            if (levelFundRewardResponse != null && levelFundRewardResponse.Code == 0)
            {
                EventArgsRefreshIAPLevelFundRewards instance = Singleton<EventArgsRefreshIAPLevelFundRewards>.Instance;
                instance.SetData(levelFundRewardResponse.LevelFundReward, levelFundRewardResponse.FreeLevelFundReward);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshIAPLevelFundRewards, instance);
                Action<bool, LevelFundRewardResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, levelFundRewardResponse);
                return;
            }
            else
            {
                Action<bool, LevelFundRewardResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, levelFundRewardResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void ShopGetInfoRequest(Action<bool, ShopGetInfoResponse> callback)
    {
        ShopGetInfoRequest shopGetInfoRequest = new ShopGetInfoRequest();
        shopGetInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(shopGetInfoRequest, delegate (IMessage response)
        {
            ShopGetInfoResponse shopGetInfoResponse = response as ShopGetInfoResponse;
            if (shopGetInfoResponse != null && shopGetInfoResponse.Code == 0)
            {
                Action<bool, ShopGetInfoResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, shopGetInfoResponse);
                return;
            }
            else
            {
                Action<bool, ShopGetInfoResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, shopGetInfoResponse);
                return;
            }
        }, false, false, string.Empty, true);
    }

    public static void SendMonthCardGetRewardRequest(int tableID, Action<bool, MonthCardGetRewardResponse> callback)
    {
        MonthCardGetRewardRequest monthCardGetRewardRequest = new MonthCardGetRewardRequest();
        monthCardGetRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
        monthCardGetRewardRequest.MonthCardId = (uint)tableID;
        GameApp.NetWork.Send(monthCardGetRewardRequest, delegate (IMessage response)
        {
            MonthCardGetRewardResponse monthCardGetRewardResponse = response as MonthCardGetRewardResponse;
            if (monthCardGetRewardResponse != null && monthCardGetRewardResponse.Code == 0)
            {
                EventArgsRefreshIAPInfoData instance = Singleton<EventArgsRefreshIAPInfoData>.Instance;
                instance.SetData(monthCardGetRewardResponse.IapInfo);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshIAPInfoData, instance);
                Action<bool, MonthCardGetRewardResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, monthCardGetRewardResponse);
                return;
            }
            else
            {
                Action<bool, MonthCardGetRewardResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, monthCardGetRewardResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void SendBuyShopFreeIAPItemRequest(int purchaseID, int extraType, string extraInfo, Action<bool, ShopFreeIAPItemResponse> callback)
    {
        ShopFreeIAPItemRequest shopFreeIAPItemRequest = new ShopFreeIAPItemRequest
        {
            CommonParams = NetworkUtils.GetCommonParams(),
            ConfigId = (uint)purchaseID,
            ExtraType = (uint)extraType,
            ExtraInfo = extraInfo
        };
        GameApp.NetWork.Send(shopFreeIAPItemRequest, delegate (IMessage response)
        {
            ShopFreeIAPItemResponse shopFreeIAPItemResponse = response as ShopFreeIAPItemResponse;
            if (shopFreeIAPItemResponse != null && shopFreeIAPItemResponse.Code == 0)
            {
                EventArgsRefreshIAPCommonData instance = Singleton<EventArgsRefreshIAPCommonData>.Instance;
                instance.SetData(shopFreeIAPItemResponse.RechargeIds, false);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshCommonData, instance);
                EventArgsRefreshIAPInfoData instance2 = Singleton<EventArgsRefreshIAPInfoData>.Instance;
                instance2.SetData(shopFreeIAPItemResponse.IapInfo);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshIAPInfoData, instance2);
                Action<bool, ShopFreeIAPItemResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, shopFreeIAPItemResponse);
                return;
            }
            else
            {
                Action<bool, ShopFreeIAPItemResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, shopFreeIAPItemResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void SendFirstRechargeRewardRequest(Action<bool, FirstRechargeRewardResponse> callback)
    {
        FirstRechargeRewardRequest firstRechargeRewardRequest = new FirstRechargeRewardRequest
        {
            CommonParams = NetworkUtils.GetCommonParams()
        };
        GameApp.NetWork.Send(firstRechargeRewardRequest, delegate (IMessage response)
        {
            FirstRechargeRewardResponse firstRechargeRewardResponse = response as FirstRechargeRewardResponse;
            if (firstRechargeRewardResponse != null && firstRechargeRewardResponse.Code == 0)
            {
                EventArgsRefreshIAPFirstGiftData instance = Singleton<EventArgsRefreshIAPFirstGiftData>.Instance;
                instance.SetData(firstRechargeRewardResponse.FirstRechargeReward, firstRechargeRewardResponse.TotalRecharge);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshIAPFirstGiftData, instance);
                Action<bool, FirstRechargeRewardResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, firstRechargeRewardResponse);
                return;
            }
            else
            {
                Action<bool, FirstRechargeRewardResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, firstRechargeRewardResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void SendVIPLevelRewardRequest(int viplevel, Action<bool, VIPLevelRewardResponse> callback)
    {
        VIPLevelRewardRequest viplevelRewardRequest = new VIPLevelRewardRequest();
        viplevelRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
        viplevelRewardRequest.Level = (uint)viplevel;
        GameApp.NetWork.Send(viplevelRewardRequest, delegate (IMessage response)
        {
            VIPLevelRewardResponse viplevelRewardResponse = response as VIPLevelRewardResponse;
            if (viplevelRewardResponse != null && viplevelRewardResponse.Code == 0)
            {
                Action<bool, VIPLevelRewardResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, viplevelRewardResponse);
                return;
            }
            else
            {
                Action<bool, VIPLevelRewardResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, viplevelRewardResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoFirstRechargeRewardV1Request(int tableId, int day, Action<bool, FirstRechargeRewardV1Response> callback)
    {
        FirstRechargeRewardV1Request firstRechargeRewardV1Request = new FirstRechargeRewardV1Request
        {
            CommonParams = NetworkUtils.GetCommonParams(),
            Configid = tableId,
            Products = day
        };
        GameApp.NetWork.Send(firstRechargeRewardV1Request, delegate (IMessage response)
        {
            FirstRechargeRewardV1Response firstRechargeRewardV1Response = response as FirstRechargeRewardV1Response;
            if (firstRechargeRewardV1Response != null && firstRechargeRewardV1Response.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.IAPDataModule).MeetingGift.UpdateRewardSign(firstRechargeRewardV1Response.FirstChargeGiftReward);
                Action<bool, FirstRechargeRewardV1Response> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, firstRechargeRewardV1Response);
                return;
            }
            else
            {
                Action<bool, FirstRechargeRewardV1Response> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, firstRechargeRewardV1Response);
                return;
            }
        }, true, false, string.Empty, true);
    }
}
