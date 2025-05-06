
public class ChainPacks
{
    public static void DoChainPacksTimeRequest(bool isLoginRequest = false, bool isShowMask = true, Action<bool, int> callback = null, bool showError = true)
    {
        if (isLoginRequest)
        {
            isShowMask = false;
            showError = false;
        }
        getChainPacksTimeRequest getChainPacksTimeRequest = new getChainPacksTimeRequest
        {
            CommonParams = NetworkUtils.GetCommonParams()
        };
        GameApp.NetWork.Send(getChainPacksTimeRequest, delegate (IMessage response)
        {
            getChainPacksTimeResponse getChainPacksTimeResponse = response as getChainPacksTimeResponse;
            if (getChainPacksTimeResponse != null)
            {
                if (getChainPacksTimeResponse.Code == 0)
                {
                    GameApp.Data.GetDataModule(DataName.ChainPacksDataModule).NetUpdateChainPacksInfo(getChainPacksTimeResponse.ChainActvDto);
                    Action<bool, int> callback2 = callback;
                    if (callback2 == null)
                    {
                        return;
                    }
                    callback2(true, getChainPacksTimeResponse.Code);
                    return;
                }
                else
                {
                    Action<bool, int> callback3 = callback;
                    if (callback3 == null)
                    {
                        return;
                    }
                    callback3(false, getChainPacksTimeResponse.Code);
                    return;
                }
            }
            else
            {
                Action<bool, int> callback4 = callback;
                if (callback4 == null)
                {
                    return;
                }
                callback4(false, -106);
                return;
            }
        }, isShowMask, false, string.Empty, showError);
    }

    public static void DoFreePickChainPacksRewardRequest(int actId, int pkId, bool isShowMask = true, Action<bool, int> callback = null)
    {
        takeChainPacksRewardRequest takeChainPacksRewardRequest = new takeChainPacksRewardRequest
        {
            Id = pkId,
            CommonParams = NetworkUtils.GetCommonParams(),
            ActId = actId
        };
        GameApp.NetWork.Send(takeChainPacksRewardRequest, delegate (IMessage response)
        {
            takeChainPacksRewardResponse takeChainPacksRewardResponse = response as takeChainPacksRewardResponse;
            if (takeChainPacksRewardResponse != null)
            {
                if (takeChainPacksRewardResponse.Code == 0)
                {
                    if (takeChainPacksRewardResponse.CommonData != null && takeChainPacksRewardResponse.CommonData.Reward != null && takeChainPacksRewardResponse.CommonData.Reward.Count > 0)
                    {
                        DxxTools.UI.OpenRewardCommon(takeChainPacksRewardResponse.CommonData.Reward, null, true);
                    }
                    GameApp.Data.GetDataModule(DataName.ChainPacksDataModule).NetPickedReward(actId, pkId);
                    Action<bool, int> callback2 = callback;
                    if (callback2 == null)
                    {
                        return;
                    }
                    callback2(true, takeChainPacksRewardResponse.Code);
                    return;
                }
                else
                {
                    Action<bool, int> callback3 = callback;
                    if (callback3 == null)
                    {
                        return;
                    }
                    callback3(false, takeChainPacksRewardResponse.Code);
                    return;
                }
            }
            else
            {
                Action<bool, int> callback4 = callback;
                if (callback4 == null)
                {
                    return;
                }
                callback4(false, -106);
                return;
            }
        }, isShowMask, false, string.Empty, true);
    }

    public static void DoFreePickChainPacksPushRewardRequest(int actId, int pkId, bool isShowMask = true, Action<bool, int> callback = null)
    {
        takePushChainRewardRequest takePushChainRewardRequest = new takePushChainRewardRequest
        {
            Id = pkId,
            CommonParams = NetworkUtils.GetCommonParams(),
            ActId = actId
        };
        GameApp.NetWork.Send(takePushChainRewardRequest, delegate (IMessage response)
        {
            takePushChainRewardResponse takePushChainRewardResponse = response as takePushChainRewardResponse;
            if (takePushChainRewardResponse != null)
            {
                if (takePushChainRewardResponse.Code == 0)
                {
                    if (takePushChainRewardResponse.CommonData != null && takePushChainRewardResponse.CommonData.Reward != null && takePushChainRewardResponse.CommonData.Reward.Count > 0)
                    {
                        DxxTools.UI.OpenRewardCommon(takePushChainRewardResponse.CommonData.Reward, null, true);
                    }
                    GameApp.Data.GetDataModule(DataName.ChainPacksDataModule).NetPickedReward(actId, pkId);
                    Action<bool, int> callback2 = callback;
                    if (callback2 == null)
                    {
                        return;
                    }
                    callback2(true, takePushChainRewardResponse.Code);
                    return;
                }
                else
                {
                    Action<bool, int> callback3 = callback;
                    if (callback3 == null)
                    {
                        return;
                    }
                    callback3(false, takePushChainRewardResponse.Code);
                    return;
                }
            }
            else
            {
                Action<bool, int> callback4 = callback;
                if (callback4 == null)
                {
                    return;
                }
                callback4(false, -106);
                return;
            }
        }, isShowMask, false, string.Empty, true);
    }
}
