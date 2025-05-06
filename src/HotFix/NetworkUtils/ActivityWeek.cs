
public class ActivityWeek
{
    public static void RequestActTimeActivityList(bool showMask = true)
    {
        ActTimeInfoRequest actTimeInfoRequest = new ActTimeInfoRequest();
        actTimeInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(actTimeInfoRequest, delegate (IMessage response)
        {
            ActTimeInfoResponse actTimeInfoResponse = response as ActTimeInfoResponse;
            if (actTimeInfoResponse != null && actTimeInfoResponse.Code == 0)
            {
                EventArgsActTimeInfoData instance = Singleton<EventArgsActTimeInfoData>.Instance;
                instance.SetData(actTimeInfoResponse);
                GameApp.Event.DispatchNow(null, 253, instance);
            }
        }, showMask, false, string.Empty, false);
    }

    public static void RequestActTimeRank(int page, bool isNextPage, int actId, bool isShowMask, Action<int, bool, bool, ActTimeRankResponse> callback)
    {
        ActTimeRankRequest actTimeRankRequest = new ActTimeRankRequest();
        actTimeRankRequest.CommonParams = NetworkUtils.GetCommonParams();
        actTimeRankRequest.ActId = actId;
        actTimeRankRequest.Page = page;
        GameApp.NetWork.Send(actTimeRankRequest, delegate (IMessage response)
        {
            ActTimeRankResponse actTimeRankResponse = response as ActTimeRankResponse;
            if (actTimeRankResponse != null && actTimeRankResponse.Code == 0)
            {
                EventArgsActTimeRankData instance = Singleton<EventArgsActTimeRankData>.Instance;
                instance.SetData(actTimeRankResponse);
                GameApp.Event.DispatchNow(null, 254, instance);
                if (callback != null)
                {
                    callback(page, isNextPage, true, actTimeRankResponse);
                    return;
                }
            }
            else if (callback != null)
            {
                callback(page, isNextPage, false, actTimeRankResponse);
            }
        }, isShowMask, false, string.Empty, true);
    }

    public static void RequestActiveShopBug(int actId, int id, Action<bool, ActShopBuyResponse> callback)
    {
        ActShopBuyRequest actShopBuyRequest = new ActShopBuyRequest();
        actShopBuyRequest.Id = id;
        actShopBuyRequest.ActId = actId;
        actShopBuyRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(actShopBuyRequest, delegate (IMessage response)
        {
            ActShopBuyResponse actShopBuyResponse = response as ActShopBuyResponse;
            if (actShopBuyResponse != null && actShopBuyResponse.Code == 0)
            {
                if (actShopBuyResponse.CommonData != null && actShopBuyResponse.CommonData.Reward != null && actShopBuyResponse.CommonData.Reward.Count > 0)
                {
                    DxxTools.UI.OpenRewardCommon(actShopBuyResponse.CommonData.Reward, null, true);
                }
                NetworkUtils.ActivityWeek.RequestActTimeActivityList(true);
                if (callback != null)
                {
                    callback(true, actShopBuyResponse);
                    return;
                }
            }
            else if (callback != null)
            {
                callback(false, actShopBuyResponse);
            }
        }, true, false, string.Empty, true);
    }

    public static void RequestActiveDropBuy(int actId, int id, Action<bool, ActDropBuyResponse> callback)
    {
        ActDropBuyRequest actDropBuyRequest = new ActDropBuyRequest();
        actDropBuyRequest.Id = id;
        actDropBuyRequest.ActId = actId;
        actDropBuyRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(actDropBuyRequest, delegate (IMessage response)
        {
            ActDropBuyResponse actDropBuyResponse = response as ActDropBuyResponse;
            if (actDropBuyResponse != null && actDropBuyResponse.Code == 0)
            {
                if (actDropBuyResponse.CommonData != null && actDropBuyResponse.CommonData.Reward != null && actDropBuyResponse.CommonData.Reward.Count > 0)
                {
                    DxxTools.UI.OpenRewardCommon(actDropBuyResponse.CommonData.Reward, null, true);
                }
                NetworkUtils.ActivityWeek.RequestActTimeActivityList(true);
                if (callback != null)
                {
                    callback(true, actDropBuyResponse);
                    return;
                }
            }
            else if (callback != null)
            {
                callback(false, actDropBuyResponse);
            }
        }, true, false, string.Empty, true);
    }

    public static void RequestActTimePayFreeReward(int actId, int cfgId, Action<bool, ActTimePayFreeRewardResponse> callback)
    {
        ActTimePayFreeRewardRequest actTimePayFreeRewardRequest = new ActTimePayFreeRewardRequest();
        actTimePayFreeRewardRequest.ActId = actId;
        actTimePayFreeRewardRequest.ConfigId = cfgId;
        actTimePayFreeRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(actTimePayFreeRewardRequest, delegate (IMessage response)
        {
            ActTimePayFreeRewardResponse actTimePayFreeRewardResponse = response as ActTimePayFreeRewardResponse;
            if (actTimePayFreeRewardResponse != null && actTimePayFreeRewardResponse.Code == 0)
            {
                if (actTimePayFreeRewardResponse.CommonData != null && actTimePayFreeRewardResponse.CommonData.Reward != null && actTimePayFreeRewardResponse.CommonData.Reward.Count > 0)
                {
                    DxxTools.UI.OpenRewardCommon(actTimePayFreeRewardResponse.CommonData.Reward, null, true);
                }
                NetworkUtils.ActivityWeek.RequestActTimeActivityList(true);
                if (callback != null)
                {
                    callback(true, actTimePayFreeRewardResponse);
                    return;
                }
            }
            else if (callback != null)
            {
                callback(false, actTimePayFreeRewardResponse);
            }
        }, true, false, string.Empty, true);
    }

    public static void RequestActTimeReward(int actId, int opt, int taskId, Action<bool, ActTimeRewardResponse> callback)
    {
        ActTimeRewardRequest actTimeRewardRequest = new ActTimeRewardRequest();
        actTimeRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
        actTimeRewardRequest.ActId = actId;
        actTimeRewardRequest.Opt = opt;
        actTimeRewardRequest.TaskId = taskId;
        GameApp.NetWork.Send(actTimeRewardRequest, delegate (IMessage response)
        {
            ActTimeRewardResponse actTimeRewardResponse = response as ActTimeRewardResponse;
            if (actTimeRewardResponse != null && actTimeRewardResponse.Code == 0)
            {
                if (actTimeRewardResponse.CommonData != null && actTimeRewardResponse.CommonData.Reward != null && actTimeRewardResponse.CommonData.Reward.Count > 0)
                {
                    DxxTools.UI.OpenRewardCommon(actTimeRewardResponse.CommonData.Reward, null, true);
                    GameApp.SDK.Analyze.Track_TaskFinish("周活动", actId, taskId, actTimeRewardResponse.CommonData.Reward);
                }
                NetworkUtils.ActivityWeek.RequestActTimeActivityList(true);
                if (callback != null)
                {
                    callback(true, actTimeRewardResponse);
                    return;
                }
            }
            else if (callback != null)
            {
                callback(false, actTimeRewardResponse);
            }
        }, true, false, string.Empty, true);
    }
}
