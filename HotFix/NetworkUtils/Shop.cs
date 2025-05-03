
public class Shop
{
    public static void IntegralShopGetInfoRequest(ShopType shopType, Action<bool, IntegralShopGetInfoResponse> callback)
    {
        IntegralShopGetInfoRequest integralShopGetInfoRequest = new IntegralShopGetInfoRequest
        {
            CommonParams = NetworkUtils.GetCommonParams(),
            ShopConfigId = (int)shopType
        };
        GameApp.NetWork.Send(integralShopGetInfoRequest, delegate (IMessage response)
        {
            IntegralShopGetInfoResponse integralShopGetInfoResponse = response as IntegralShopGetInfoResponse;
            bool flag = integralShopGetInfoResponse != null && integralShopGetInfoResponse.Code == 0;
            if (flag)
            {
                if (integralShopGetInfoResponse.Shop != null)
                {
                    EventArgsRefreshShopData instance = Singleton<EventArgsRefreshShopData>.Instance;
                    instance.ShopType = (ShopType)integralShopGetInfoResponse.Shop.ShopConfigId;
                    instance.ShopInfo = integralShopGetInfoResponse.Shop;
                    GameApp.Event.DispatchNow(null, LocalMessageName.CC_ShopDataMoudule_RefreshShopData, instance);
                }
                RedPointController.Instance.ReCalc("MainShop", true);
            }
            Action<bool, IntegralShopGetInfoResponse> callback2 = callback;
            if (callback2 == null)
            {
                return;
            }
            callback2(flag, integralShopGetInfoResponse);
        }, true, false, string.Empty, true);
    }

    public static void IntegralShopRefreshRequest(ShopType shopType, int refreshCurrencyID, int refreshPrice, Action<bool, IntegralShopRefreshResponse> callback)
    {
        IntegralShopRefreshRequest integralShopRefreshRequest = new IntegralShopRefreshRequest
        {
            CommonParams = NetworkUtils.GetCommonParams(),
            ShopConfigId = (int)shopType
        };
        GameApp.NetWork.Send(integralShopRefreshRequest, delegate (IMessage response)
        {
            IntegralShopRefreshResponse integralShopRefreshResponse = response as IntegralShopRefreshResponse;
            bool flag = integralShopRefreshResponse != null && integralShopRefreshResponse.Code == 0;
            if (flag)
            {
                EventArgsRefreshShopData instance = Singleton<EventArgsRefreshShopData>.Instance;
                instance.ShopType = (ShopType)integralShopRefreshResponse.Shop.ShopConfigId;
                instance.ShopInfo = integralShopRefreshResponse.Shop;
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_ShopDataMoudule_RefreshShopData, instance);
                RedPointController.Instance.ReCalc("MainShop", true);
                RedPointController.Instance.ReCalc("Main.BlackMarket", true);
            }
            Action<bool, IntegralShopRefreshResponse> callback2 = callback;
            if (callback2 == null)
            {
                return;
            }
            callback2(flag, integralShopRefreshResponse);
        }, true, false, string.Empty, true);
    }

    public static void IntegralShopBuyRequest(ShopType shopType, int configId, int buyCurrencyID, int buyPrice, Action<bool, IntegralShopBuyResponse> callback)
    {
        IntegralShopBuyRequest integralShopBuyRequest = new IntegralShopBuyRequest
        {
            CommonParams = NetworkUtils.GetCommonParams(),
            GoodsConfigId = configId,
            ShopConfigId = (int)shopType
        };
        GameApp.NetWork.Send(integralShopBuyRequest, delegate (IMessage response)
        {
            IntegralShopBuyResponse integralShopBuyResponse = response as IntegralShopBuyResponse;
            if (integralShopBuyResponse != null && integralShopBuyResponse.Code == 0)
            {
                EventArgsBuyShopItem instance = Singleton<EventArgsBuyShopItem>.Instance;
                instance.ShopItemId = integralShopBuyResponse.GoodsConfigId;
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_ShopDataMoudule_BuyShopItem, instance);
                RedPointController.Instance.ReCalc("MainShop", true);
                RedPointController.Instance.ReCalc("Equip", true);
                RedPointController.Instance.ReCalc("Main.BlackMarket", true);
            }
            Action<bool, IntegralShopBuyResponse> callback2 = callback;
            if (callback2 == null)
            {
                return;
            }
            callback2(integralShopBuyResponse != null && integralShopBuyResponse.Code == 0, integralShopBuyResponse);
        }, true, false, string.Empty, true);
    }

    public static void ShopBuyItemRequest(int configId, int costType, Action<bool, ShopBuyItemResponse> callback)
    {
        ShopBuyItemRequest shopBuyItemRequest = new ShopBuyItemRequest();
        shopBuyItemRequest.CommonParams = NetworkUtils.GetCommonParams();
        shopBuyItemRequest.ConfigId = (uint)configId;
        shopBuyItemRequest.BuyType = (uint)costType;
        GameApp.NetWork.Send(shopBuyItemRequest, delegate (IMessage resp)
        {
            ShopBuyItemResponse shopBuyItemResponse = resp as ShopBuyItemResponse;
            if (shopBuyItemResponse != null && shopBuyItemResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.AdDataModule).UpdateAdData(shopBuyItemResponse.AdData);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_ShopDataMoudule_RefreshShopData, null);
                RedPointController.Instance.ReCalc("MainShop", true);
                RedPointController.Instance.ReCalc("Equip", true);
                RedPointController.Instance.ReCalc("Main.BlackMarket", true);
                DxxTools.UI.OpenRewardCommon(shopBuyItemResponse.CommonData.Reward, null, true);
            }
            Action<bool, ShopBuyItemResponse> callback2 = callback;
            if (callback2 == null)
            {
                return;
            }
            callback2(shopBuyItemResponse != null && shopBuyItemResponse.Code == 0, shopBuyItemResponse);
        }, true, false, string.Empty, true);
    }

    public static void ShopDoDrawRequest(int summonId, int costType, int drawType, Action<bool, ShopDoDrawResponse> callback)
    {
        ShopDoDrawRequest shopDoDrawRequest = new ShopDoDrawRequest();
        shopDoDrawRequest.CommonParams = NetworkUtils.GetCommonParams();
        shopDoDrawRequest.SummonId = (uint)summonId;
        shopDoDrawRequest.CostType = (uint)costType;
        shopDoDrawRequest.DrawType = (uint)drawType;
        GameApp.NetWork.Send(shopDoDrawRequest, delegate (IMessage response)
        {
            ShopDoDrawResponse shopDoDrawResponse = response as ShopDoDrawResponse;
            if (shopDoDrawResponse != null && shopDoDrawResponse.Code.Equals(0))
            {
                IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
                dataModule.UpdateShopDrawCount(shopDoDrawResponse.ShopDrawDto);
                dataModule.UpdateShopSUpPoolDrawCount(shopDoDrawResponse.ShopSupCount);
                GameApp.Data.GetDataModule(DataName.AdDataModule).UpdateAdData(shopDoDrawResponse.AdData);
                RedPointController.Instance.ReCalc("MainShop", true);
                Action<bool, ShopDoDrawResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, shopDoDrawResponse);
                return;
            }
            else
            {
                Action<bool, ShopDoDrawResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, null);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void FinishAdvertRequest(int adId, Action<bool, FinishAdvertResponse> callback)
    {
        FinishAdvertRequest finishAdvertRequest = new FinishAdvertRequest();
        finishAdvertRequest.CommonParams = NetworkUtils.GetCommonParams();
        finishAdvertRequest.AdId = adId;
        GameApp.NetWork.Send(finishAdvertRequest, delegate (IMessage response)
        {
            FinishAdvertResponse finishAdvertResponse = response as FinishAdvertResponse;
            if (finishAdvertResponse != null && finishAdvertResponse.Code.Equals(0))
            {
                GameApp.Data.GetDataModule(DataName.AdDataModule).UpdateAdData(finishAdvertResponse.AdData);
                Action<bool, FinishAdvertResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, finishAdvertResponse);
                return;
            }
            else
            {
                Action<bool, FinishAdvertResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, null);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoHeadFrameActiveRequest(Action<bool, HeadFrameActiveResponse> callback)
    {
        HeadFrameActiveRequest headFrameActiveRequest = new HeadFrameActiveRequest();
        headFrameActiveRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(headFrameActiveRequest, delegate (IMessage response)
        {
            HeadFrameActiveResponse headFrameActiveResponse = response as HeadFrameActiveResponse;
            if (headFrameActiveResponse != null && headFrameActiveResponse.Code.Equals(0))
            {
                GameApp.Data.GetDataModule(DataName.IAPDataModule).MonthCard.SetHeadState(headFrameActiveResponse.IsHeadFrameActive);
                Action<bool, HeadFrameActiveResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, headFrameActiveResponse);
                return;
            }
            else
            {
                Action<bool, HeadFrameActiveResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, null);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoBuyItemRewardRequest(int configId, int equipId, Action<bool, BuyItemRewardResponse> callback)
    {
        BuyItemRewardRequest buyItemRewardRequest = new BuyItemRewardRequest();
        buyItemRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
        buyItemRewardRequest.ConfigId = configId;
        buyItemRewardRequest.EquipId = equipId;
        GameApp.NetWork.Send(buyItemRewardRequest, delegate (IMessage response)
        {
            BuyItemRewardResponse buyItemRewardResponse = response as BuyItemRewardResponse;
            if (buyItemRewardResponse != null && buyItemRewardResponse.Code.Equals(0))
            {
                GameApp.Data.GetDataModule(DataName.IAPDataModule).UpdateShopSUpPoolDrawCount(buyItemRewardResponse.ShopSupCount);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_ShopDataMoudule_RefreshShopData, null);
                RedPointController.Instance.ReCalc("MainShop", true);
                DxxTools.UI.OpenRewardCommon(buyItemRewardResponse.CommonData.Reward, null, true);
                Action<bool, BuyItemRewardResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, buyItemRewardResponse);
                return;
            }
            else
            {
                Action<bool, BuyItemRewardResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, null);
                return;
            }
        }, true, false, string.Empty, true);
    }
}
