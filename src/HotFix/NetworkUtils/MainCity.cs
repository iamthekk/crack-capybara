
public class MainCity
{
    public static void DoCityGoldmineLevelUpRequest(Action<bool, CityGoldmineLevelUpResponse> callback)
    {
        CityGoldmineLevelUpRequest cityGoldmineLevelUpRequest = new CityGoldmineLevelUpRequest();
        cityGoldmineLevelUpRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(cityGoldmineLevelUpRequest, delegate (IMessage response)
        {
            CityGoldmineLevelUpResponse cityGoldmineLevelUpResponse = response as CityGoldmineLevelUpResponse;
            if (cityGoldmineLevelUpResponse != null && cityGoldmineLevelUpResponse.Code == 0)
            {
                EventArgsRefreshMainCityGoldData instance = Singleton<EventArgsRefreshMainCityGoldData>.Instance;
                instance.SetData(cityGoldmineLevelUpResponse.Level, 0L);
                GameApp.Event.DispatchNow(null, 142, instance);
                Action<bool, CityGoldmineLevelUpResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, cityGoldmineLevelUpResponse);
                return;
            }
            else
            {
                Action<bool, CityGoldmineLevelUpResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, cityGoldmineLevelUpResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoCityGoldmineHangRewardRequest(Action<bool, CityGoldmineHangRewardResponse> callback)
    {
        CityGoldmineHangRewardRequest cityGoldmineHangRewardRequest = new CityGoldmineHangRewardRequest();
        cityGoldmineHangRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(cityGoldmineHangRewardRequest, delegate (IMessage response)
        {
            CityGoldmineHangRewardResponse cityGoldmineHangRewardResponse = response as CityGoldmineHangRewardResponse;
            if (cityGoldmineHangRewardResponse != null && cityGoldmineHangRewardResponse.Code == 0)
            {
                MainCityDataModule dataModule = GameApp.Data.GetDataModule(DataName.MainCityDataModule);
                ulong lastGoldmineRewardTime = cityGoldmineHangRewardResponse.LastGoldmineRewardTime;
                long goldTimeSpan = dataModule.m_goldTimeSpan;
                EventArgsRefreshMainCityGoldData instance = Singleton<EventArgsRefreshMainCityGoldData>.Instance;
                instance.SetData(0, (long)cityGoldmineHangRewardResponse.LastGoldmineRewardTime);
                GameApp.Event.DispatchNow(null, 142, instance);
                Action<bool, CityGoldmineHangRewardResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, cityGoldmineHangRewardResponse);
                return;
            }
            else
            {
                Action<bool, CityGoldmineHangRewardResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, cityGoldmineHangRewardResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoCityGetChestInfoRequest(Action<bool, CityGetChestInfoResponse> callback)
    {
        CityGetChestInfoRequest cityGetChestInfoRequest = new CityGetChestInfoRequest();
        cityGetChestInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(cityGetChestInfoRequest, delegate (IMessage response)
        {
            CityGetChestInfoResponse cityGetChestInfoResponse = response as CityGetChestInfoResponse;
            if (cityGetChestInfoResponse != null && cityGetChestInfoResponse.Code == 0)
            {
                EventArgsRefreshMainCityBoxData instance = Singleton<EventArgsRefreshMainCityBoxData>.Instance;
                instance.SetData(cityGetChestInfoResponse.CityChest, cityGetChestInfoResponse.StartTime, cityGetChestInfoResponse.RefreshTime);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_MainCityData_RefreshMainCityBoxData, instance);
                Action<bool, CityGetChestInfoResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, cityGetChestInfoResponse);
                return;
            }
            else
            {
                Action<bool, CityGetChestInfoResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, cityGetChestInfoResponse);
                return;
            }
        }, false, false, string.Empty, true);
    }

    public static void DoCityOpenChestRequest(List<ulong> rowIDs, Action<bool, CityOpenChestResponse> callback, bool isShowMask = true)
    {
        CityOpenChestRequest cityOpenChestRequest = new CityOpenChestRequest();
        cityOpenChestRequest.CommonParams = NetworkUtils.GetCommonParams();
        cityOpenChestRequest.RowId.AddRange(rowIDs);
        GameApp.NetWork.Send(cityOpenChestRequest, delegate (IMessage response)
        {
            CityOpenChestResponse cityOpenChestResponse = response as CityOpenChestResponse;
            if (cityOpenChestResponse != null && cityOpenChestResponse.Code == 0)
            {
                EventArgsRefreshMainCityBoxData instance = Singleton<EventArgsRefreshMainCityBoxData>.Instance;
                instance.SetData(cityOpenChestResponse.RowId, cityOpenChestResponse.RefreshTime, cityOpenChestResponse.Score);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_MainCityData_RefreshMainCityBoxData, instance);
                RedPointController.Instance.ReCalc("Main.Box", true);
                Action<bool, CityOpenChestResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, cityOpenChestResponse);
                return;
            }
            else
            {
                Action<bool, CityOpenChestResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, cityOpenChestResponse);
                return;
            }
        }, isShowMask, false, string.Empty, true);
    }

    public static void DoCityTakeScoreRewardRequest(Action<bool, CityTakeScoreRewardResponse> callback)
    {
        CityTakeScoreRewardRequest cityTakeScoreRewardRequest = new CityTakeScoreRewardRequest();
        cityTakeScoreRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(cityTakeScoreRewardRequest, delegate (IMessage response)
        {
            CityTakeScoreRewardResponse cityTakeScoreRewardResponse = response as CityTakeScoreRewardResponse;
            if (cityTakeScoreRewardResponse != null && cityTakeScoreRewardResponse.Code == 0)
            {
                EventArgsRefreshMainCityBoxData instance = Singleton<EventArgsRefreshMainCityBoxData>.Instance;
                instance.SetData(cityTakeScoreRewardResponse.RefreshTime, cityTakeScoreRewardResponse.Score);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_MainCityData_RefreshMainCityBoxData, instance);
                RedPointController.Instance.ReCalc("Main.Box", true);
                Action<bool, CityTakeScoreRewardResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, cityTakeScoreRewardResponse);
                return;
            }
            else
            {
                Action<bool, CityTakeScoreRewardResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, cityTakeScoreRewardResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoCitySyncPowerRequest(long power)
    {
        NetworkUtils.MainCity.newPower = power;
        if (NetworkUtils.MainCity.inSendingPower)
        {
            return;
        }
        NetworkUtils.MainCity.inSendingPower = true;
        DelayCall.Instance.CallOnce(2000, delegate
        {
            NetworkUtils.MainCity.inSendingPower = false;
            CitySyncPowerRequest citySyncPowerRequest = new CitySyncPowerRequest();
            citySyncPowerRequest.CommonParams = NetworkUtils.GetCommonParams();
            citySyncPowerRequest.Power = (long)((int)power);
            citySyncPowerRequest.ClientVersion = Singleton<BattleServerVersionMgr>.Instance.GetVersion();
            GameApp.NetWork.Send(citySyncPowerRequest, delegate (IMessage response)
            {
                CitySyncPowerResponse citySyncPowerResponse = response as CitySyncPowerResponse;
                if (citySyncPowerResponse != null)
                {
                    int code = citySyncPowerResponse.Code;
                }
            }, false, false, string.Empty, false);
        });
    }

    public static void DoCityGetInfoRequest(Action<bool, CityGetInfoResponse> callback)
    {
        CityGetInfoRequest cityGetInfoRequest = new CityGetInfoRequest();
        cityGetInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(cityGetInfoRequest, delegate (IMessage response)
        {
            CityGetInfoResponse cityGetInfoResponse = response as CityGetInfoResponse;
            if (cityGetInfoResponse != null && cityGetInfoResponse.Code == 0)
            {
                Action<bool, CityGetInfoResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, cityGetInfoResponse);
                return;
            }
            else
            {
                Action<bool, CityGetInfoResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, cityGetInfoResponse);
                return;
            }
        }, false, false, string.Empty, false);
    }

    private static long newPower;

    private static bool inSendingPower;
}
