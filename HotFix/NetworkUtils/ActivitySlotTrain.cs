
public class ActivitySlotTrain
{
    public static void RequestTurnTableGetInfo(bool isShowMask = true, Action<bool, int> callback = null)
    {
        TurnTableGetInfoRequest turnTableGetInfoRequest = new TurnTableGetInfoRequest();
        turnTableGetInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(turnTableGetInfoRequest, delegate (IMessage response)
        {
            TurnTableGetInfoResponse turnTableGetInfoResponse = response as TurnTableGetInfoResponse;
            if (turnTableGetInfoResponse != null)
            {
                if (turnTableGetInfoResponse.Code == 0)
                {
                    EventArgsActivitySlotTrainData instance = Singleton<EventArgsActivitySlotTrainData>.Instance;
                    instance.SetData(turnTableGetInfoResponse);
                    GameApp.Event.DispatchNow(null, LocalMessageName.CC_ActivitySlotTrain, instance);
                    Action<bool, int> callback2 = callback;
                    if (callback2 != null)
                    {
                        callback2(true, turnTableGetInfoResponse.Code);
                    }
                }
                else
                {
                    GameApp.Data.GetDataModule(DataName.ActivitySlotTrainDataModule).Clear();
                    Action<bool, int> callback3 = callback;
                    if (callback3 != null)
                    {
                        callback3(false, turnTableGetInfoResponse.Code);
                    }
                }
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_ActivitySlotTrain_StageChanged, null);
                return;
            }
            Action<bool, int> callback4 = callback;
            if (callback4 == null)
            {
                return;
            }
            callback4(false, -106);
        }, isShowMask, false, string.Empty, true);
    }

    public static void RequestTurnTableExtract(int extractNum, Action<bool, TurnTableExtractResponse> callback = null)
    {
        TurnTableExtractRequest turnTableExtractRequest = new TurnTableExtractRequest();
        turnTableExtractRequest.CommonParams = NetworkUtils.GetCommonParams();
        turnTableExtractRequest.ExtractNum = extractNum;
        GameApp.NetWork.Send(turnTableExtractRequest, delegate (IMessage response)
        {
            TurnTableExtractResponse turnTableExtractResponse = response as TurnTableExtractResponse;
            if (turnTableExtractResponse != null)
            {
                if (turnTableExtractResponse.Code == 0)
                {
                    EventArgsTurnTableExtractData instance = Singleton<EventArgsTurnTableExtractData>.Instance;
                    instance.SetData(turnTableExtractResponse);
                    GameApp.Event.DispatchNow(null, 257, instance);
                    Action<bool, TurnTableExtractResponse> callback2 = callback;
                    if (callback2 == null)
                    {
                        return;
                    }
                    callback2(true, turnTableExtractResponse);
                    return;
                }
                else
                {
                    if (turnTableExtractResponse.Code == 7001)
                    {
                        GameApp.Data.GetDataModule(DataName.ActivitySlotTrainDataModule).Clear();
                    }
                    Action<bool, TurnTableExtractResponse> callback3 = callback;
                    if (callback3 == null)
                    {
                        return;
                    }
                    callback3(false, turnTableExtractResponse);
                    return;
                }
            }
            else
            {
                Action<bool, TurnTableExtractResponse> callback4 = callback;
                if (callback4 == null)
                {
                    return;
                }
                callback4(false, null);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void RequestTurnTableReceiveCumulativeReward(int turntableRewardConfigId, Action<bool, TurnTableReceiveCumulativeRewardResponse> callback)
    {
        TurnTableReceiveCumulativeRewardRequest turnTableReceiveCumulativeRewardRequest = new TurnTableReceiveCumulativeRewardRequest();
        turnTableReceiveCumulativeRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
        turnTableReceiveCumulativeRewardRequest.TurntableRewardConfigId = turntableRewardConfigId;
        GameApp.NetWork.Send(turnTableReceiveCumulativeRewardRequest, delegate (IMessage response)
        {
            TurnTableReceiveCumulativeRewardResponse turnTableReceiveCumulativeRewardResponse = response as TurnTableReceiveCumulativeRewardResponse;
            if (turnTableReceiveCumulativeRewardResponse != null)
            {
                if (turnTableReceiveCumulativeRewardResponse.Code == 0)
                {
                    if (turnTableReceiveCumulativeRewardResponse.CommonData != null && turnTableReceiveCumulativeRewardResponse.CommonData.Reward != null && turnTableReceiveCumulativeRewardResponse.CommonData.Reward.Count > 0)
                    {
                        DxxTools.UI.OpenRewardCommon(turnTableReceiveCumulativeRewardResponse.CommonData.Reward, null, true);
                    }
                    EventArgsTurnTableReceiveCumulativeRewardData instance = Singleton<EventArgsTurnTableReceiveCumulativeRewardData>.Instance;
                    instance.SetData(turnTableReceiveCumulativeRewardResponse);
                    GameApp.Event.DispatchNow(null, 258, instance);
                    Action<bool, TurnTableReceiveCumulativeRewardResponse> callback2 = callback;
                    if (callback2 == null)
                    {
                        return;
                    }
                    callback2(true, turnTableReceiveCumulativeRewardResponse);
                    return;
                }
                else
                {
                    if (turnTableReceiveCumulativeRewardResponse.Code == 7001)
                    {
                        GameApp.Data.GetDataModule(DataName.ActivitySlotTrainDataModule).Clear();
                    }
                    Action<bool, TurnTableReceiveCumulativeRewardResponse> callback3 = callback;
                    if (callback3 == null)
                    {
                        return;
                    }
                    callback3(false, turnTableReceiveCumulativeRewardResponse);
                    return;
                }
            }
            else
            {
                Action<bool, TurnTableReceiveCumulativeRewardResponse> callback4 = callback;
                if (callback4 == null)
                {
                    return;
                }
                callback4(false, null);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void RequestTurnPayAd(int cfgId, int adID)
    {
        TurnPayAdRequest turnPayAdRequest = new TurnPayAdRequest();
        turnPayAdRequest.CommonParams = NetworkUtils.GetCommonParams();
        turnPayAdRequest.ConfigId = cfgId;
        GameApp.NetWork.Send(turnPayAdRequest, delegate (IMessage response)
        {
            TurnPayAdResponse turnPayAdResponse = response as TurnPayAdResponse;
            if (turnPayAdResponse != null && turnPayAdResponse.Code == 0)
            {
                ActivitySlotTrainDataModule dataModule = GameApp.Data.GetDataModule(DataName.ActivitySlotTrainDataModule);
                if (turnPayAdResponse.CommonData != null)
                {
                    if (turnPayAdResponse.CommonData.AdData != null)
                    {
                        GameApp.Data.GetDataModule(DataName.AdDataModule).UpdateAdData(turnPayAdResponse.CommonData.AdData);
                    }
                    if (turnPayAdResponse.CommonData.Reward != null && turnPayAdResponse.CommonData.Reward.Count > 0)
                    {
                        DxxTools.UI.OpenRewardCommon(turnPayAdResponse.CommonData.Reward, null, true);
                    }
                }
                dataModule.CommonUpdateTurntablePayCount(turnPayAdResponse.TurntablePayCount);
                GameApp.SDK.Analyze.Track_AD(GameTGATools.ADSourceName(adID), "REWARD ", "", turnPayAdResponse.CommonData.Reward, null);
            }
        }, true, false, string.Empty, true);
    }

    public static void RequestTurnTableTaskReceiveReward(int taskId)
    {
        TurnTableTaskReceiveRewardRequest turnTableTaskReceiveRewardRequest = new TurnTableTaskReceiveRewardRequest();
        turnTableTaskReceiveRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
        turnTableTaskReceiveRewardRequest.TaskId = taskId;
        GameApp.NetWork.Send(turnTableTaskReceiveRewardRequest, delegate (IMessage response)
        {
            TurnTableTaskReceiveRewardResponse turnTableTaskReceiveRewardResponse = response as TurnTableTaskReceiveRewardResponse;
            if (turnTableTaskReceiveRewardResponse != null)
            {
                if (turnTableTaskReceiveRewardResponse.Code == 0)
                {
                    if (turnTableTaskReceiveRewardResponse.CommonData != null && turnTableTaskReceiveRewardResponse.CommonData.Reward != null && turnTableTaskReceiveRewardResponse.CommonData.Reward.Count > 0)
                    {
                        DxxTools.UI.OpenRewardCommon(turnTableTaskReceiveRewardResponse.CommonData.Reward, null, true);
                        ActivitySlotTrainDataModule dataModule = GameApp.Data.GetDataModule(DataName.ActivitySlotTrainDataModule);
                        GameApp.SDK.Analyze.Track_TaskFinish("卡皮机", dataModule.TurntableId, taskId, turnTableTaskReceiveRewardResponse.CommonData.Reward);
                    }
                    EventArgsTurnTableTaskReceiveRewardData instance = Singleton<EventArgsTurnTableTaskReceiveRewardData>.Instance;
                    instance.SetData(turnTableTaskReceiveRewardResponse);
                    GameApp.Event.DispatchNow(null, 259, instance);
                    return;
                }
                if (turnTableTaskReceiveRewardResponse.Code == 7001)
                {
                    GameApp.Data.GetDataModule(DataName.ActivitySlotTrainDataModule).Clear();
                }
            }
        }, true, false, string.Empty, true);
    }

    public static void RequestTurnTableSelectBigGuaranteeItem(int itemId, Action<bool> callback = null)
    {
        TurnTableSelectBigGuaranteeItemRequest turnTableSelectBigGuaranteeItemRequest = new TurnTableSelectBigGuaranteeItemRequest();
        turnTableSelectBigGuaranteeItemRequest.CommonParams = NetworkUtils.GetCommonParams();
        turnTableSelectBigGuaranteeItemRequest.ItemId = itemId;
        GameApp.NetWork.Send(turnTableSelectBigGuaranteeItemRequest, delegate (IMessage response)
        {
            TurnTableSelectBigGuaranteeItemResponse turnTableSelectBigGuaranteeItemResponse = response as TurnTableSelectBigGuaranteeItemResponse;
            if (turnTableSelectBigGuaranteeItemResponse != null)
            {
                if (turnTableSelectBigGuaranteeItemResponse.Code == 0)
                {
                    EventArgsTurnTableSelectBigGuaranteeItemData instance = Singleton<EventArgsTurnTableSelectBigGuaranteeItemData>.Instance;
                    instance.SetData(turnTableSelectBigGuaranteeItemResponse);
                    GameApp.Event.DispatchNow(null, 260, instance);
                    Action<bool> callback2 = callback;
                    if (callback2 == null)
                    {
                        return;
                    }
                    callback2(true);
                    return;
                }
                else
                {
                    if (turnTableSelectBigGuaranteeItemResponse.Code == 7001)
                    {
                        GameApp.Data.GetDataModule(DataName.ActivitySlotTrainDataModule).Clear();
                    }
                    Action<bool> callback3 = callback;
                    if (callback3 == null)
                    {
                        return;
                    }
                    callback3(false);
                    return;
                }
            }
            else
            {
                Action<bool> callback4 = callback;
                if (callback4 == null)
                {
                    return;
                }
                callback4(false);
                return;
            }
        }, true, false, string.Empty, true);
    }
}
