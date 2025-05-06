
public class Mining
{
    public static void DoGetMiningInfoRequest(Action<bool, GetMiningInfoResponse> callback)
    {
        GetMiningInfoRequest getMiningInfoRequest = new GetMiningInfoRequest();
        getMiningInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(getMiningInfoRequest, delegate (IMessage response)
        {
            GetMiningInfoResponse getMiningInfoResponse = response as GetMiningInfoResponse;
            if (getMiningInfoResponse == null || getMiningInfoResponse.Code != 0)
            {
                Action<bool, GetMiningInfoResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(false, getMiningInfoResponse);
                }
                return;
            }
            MiningDataModule dataModule = GameApp.Data.GetDataModule(DataName.MiningDataModule);
            dataModule.UpdateMiningInfo(getMiningInfoResponse.MiningInfoDto);
            dataModule.UpdateMiningDrawInfo(getMiningInfoResponse.MiningDrawDto);
            Action<bool, GetMiningInfoResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(true, getMiningInfoResponse);
        }, true, false, string.Empty, true);
    }

    public static void DoMiningRequest(uint optType, List<int> posList, Action<bool, DoMiningResponse> callback)
    {
        DoMiningRequest doMiningRequest = new DoMiningRequest();
        doMiningRequest.CommonParams = NetworkUtils.GetCommonParams();
        doMiningRequest.OptType = (ulong)optType;
        for (int i = 0; i < posList.Count; i++)
        {
            doMiningRequest.Pos.Add(posList[i]);
        }
        GameApp.NetWork.Send(doMiningRequest, delegate (IMessage response)
        {
            DoMiningResponse doMiningResponse = response as DoMiningResponse;
            if (doMiningResponse != null)
            {
                GameApp.Data.GetDataModule(DataName.MiningDataModule).UpdateMiningInfo(doMiningResponse.MiningInfoDto);
                if (doMiningResponse.Code == 0)
                {
                    Action<bool, DoMiningResponse> callback2 = callback;
                    if (callback2 != null)
                    {
                        callback2(true, doMiningResponse);
                    }
                    GameApp.SDK.Analyze.Track_MiningMine_Mine(optType, doMiningResponse.CommonData.CostDto);
                    return;
                }
                Action<bool, DoMiningResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, doMiningResponse);
            }
        }, optType == 1U, false, string.Empty, optType != 2U);
    }

    public static void DoOpenBombRequest(int pos, Action<bool, OpenBombResponse> callback)
    {
        OpenBombRequest openBombRequest = new OpenBombRequest();
        openBombRequest.CommonParams = NetworkUtils.GetCommonParams();
        openBombRequest.Pos = pos;
        GameApp.NetWork.Send(openBombRequest, delegate (IMessage response)
        {
            OpenBombResponse openBombResponse = response as OpenBombResponse;
            if (openBombResponse != null)
            {
                GameApp.Data.GetDataModule(DataName.MiningDataModule).UpdateMiningInfo(openBombResponse.MiningInfoDto);
                if (openBombResponse.Code == 0)
                {
                    Action<bool, OpenBombResponse> callback2 = callback;
                    if (callback2 == null)
                    {
                        return;
                    }
                    callback2(true, openBombResponse);
                    return;
                }
                else
                {
                    Action<bool, OpenBombResponse> callback3 = callback;
                    if (callback3 == null)
                    {
                        return;
                    }
                    callback3(false, openBombResponse);
                }
            }
        }, false, false, string.Empty, true);
    }

    public static void DoGetMiningRewardRequest(Action<bool, GetMiningRewardResponse> callback, Action closeRewardCallback = null)
    {
        GetMiningRewardRequest getMiningRewardRequest = new GetMiningRewardRequest();
        getMiningRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(getMiningRewardRequest, delegate (IMessage response)
        {
            GetMiningRewardResponse getMiningRewardResponse = response as GetMiningRewardResponse;
            if (getMiningRewardResponse != null)
            {
                MiningDataModule dataModule = GameApp.Data.GetDataModule(DataName.MiningDataModule);
                dataModule.UpdateMiningInfo(getMiningRewardResponse.MiningInfoDto);
                GameApp.Event.DispatchNow(null, 445, null);
                if (getMiningRewardResponse.Code == 0)
                {
                    if (getMiningRewardResponse.CommonData.Reward != null && getMiningRewardResponse.CommonData.Reward.Count > 0)
                    {
                        dataModule.CacheTicket();
                        DxxTools.UI.OpenRewardCommon(getMiningRewardResponse.CommonData.Reward, closeRewardCallback, true);
                    }
                    Action<bool, GetMiningRewardResponse> callback2 = callback;
                    if (callback2 == null)
                    {
                        return;
                    }
                    callback2(true, getMiningRewardResponse);
                    return;
                }
                else
                {
                    Action<bool, GetMiningRewardResponse> callback3 = callback;
                    if (callback3 == null)
                    {
                        return;
                    }
                    callback3(false, getMiningRewardResponse);
                }
            }
        }, true, false, string.Empty, true);
    }

    public static void DoOpenNextDoorRequest(Action<bool, OpenNextDoorResponse> callback)
    {
        OpenNextDoorRequest openNextDoorRequest = new OpenNextDoorRequest();
        openNextDoorRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(openNextDoorRequest, delegate (IMessage response)
        {
            OpenNextDoorResponse openNextDoorResponse = response as OpenNextDoorResponse;
            if (openNextDoorResponse != null)
            {
                MiningDataModule dataModule = GameApp.Data.GetDataModule(DataName.MiningDataModule);
                dataModule.UpdateMiningInfo(openNextDoorResponse.MiningInfoDto);
                dataModule.CacheTicket();
                if (openNextDoorResponse.Code == 0)
                {
                    Action<bool, OpenNextDoorResponse> callback2 = callback;
                    if (callback2 == null)
                    {
                        return;
                    }
                    callback2(true, openNextDoorResponse);
                    return;
                }
                else
                {
                    Action<bool, OpenNextDoorResponse> callback3 = callback;
                    if (callback3 == null)
                    {
                        return;
                    }
                    callback3(false, openNextDoorResponse);
                }
            }
        }, true, false, string.Empty, true);
    }

    public static void DoSetMiningOptionRequest(uint autoOpt, Action<bool, SetMiningOptionResponse> callback)
    {
        SetMiningOptionRequest setMiningOptionRequest = new SetMiningOptionRequest();
        setMiningOptionRequest.CommonParams = NetworkUtils.GetCommonParams();
        setMiningOptionRequest.AutoOpt = (ulong)autoOpt;
        GameApp.NetWork.Send(setMiningOptionRequest, delegate (IMessage response)
        {
            SetMiningOptionResponse setMiningOptionResponse = response as SetMiningOptionResponse;
            if (setMiningOptionResponse != null)
            {
                GameApp.Data.GetDataModule(DataName.MiningDataModule).SetAutoOpt(setMiningOptionResponse.AutoOpt);
                if (setMiningOptionResponse.Code == 0)
                {
                    Action<bool, SetMiningOptionResponse> callback2 = callback;
                    if (callback2 == null)
                    {
                        return;
                    }
                    callback2(true, setMiningOptionResponse);
                    return;
                }
                else
                {
                    Action<bool, SetMiningOptionResponse> callback3 = callback;
                    if (callback3 == null)
                    {
                        return;
                    }
                    callback3(false, setMiningOptionResponse);
                }
            }
        }, false, false, string.Empty, true);
    }

    public static void DoBonusDrawRequest(int rate, Action<bool, BounDrawResponse> callback)
    {
        BounDrawRequest bounDrawRequest = new BounDrawRequest();
        bounDrawRequest.CommonParams = NetworkUtils.GetCommonParams();
        bounDrawRequest.Rate = rate;
        MiningDataModule miningDataModule = GameApp.Data.GetDataModule(DataName.MiningDataModule);
        bool isFree = miningDataModule.MiningDraw.FreeTimes > 0;
        int continueFreeTimes = miningDataModule.MiningDraw.ContinueFreeTimes;
        GameApp.NetWork.Send(bounDrawRequest, delegate (IMessage response)
        {
            BounDrawResponse bounDrawResponse = response as BounDrawResponse;
            if (bounDrawResponse != null)
            {
                miningDataModule.UpdateMiningDrawInfo(bounDrawResponse.DrawInfo);
                miningDataModule.CacheTicket();
                if (bounDrawResponse.Code == 0)
                {
                    Action<bool, BounDrawResponse> callback2 = callback;
                    if (callback2 != null)
                    {
                        callback2(true, bounDrawResponse);
                    }
                    GameApp.SDK.Analyze.Track_MiningTrain(isFree, continueFreeTimes, bounDrawResponse.RewardsIndex.Count, bounDrawResponse.CommonData.Reward);
                    return;
                }
                Action<bool, BounDrawResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, bounDrawResponse);
            }
        }, true, false, string.Empty, true);
    }

    public static void DoMiningBoxUpgradeRewardRequest(Action<bool, MiningBoxUpgradeRewardResponse> callback)
    {
        MiningBoxUpgradeRewardRequest miningBoxUpgradeRewardRequest = new MiningBoxUpgradeRewardRequest();
        miningBoxUpgradeRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(miningBoxUpgradeRewardRequest, delegate (IMessage response)
        {
            MiningBoxUpgradeRewardResponse miningBoxUpgradeRewardResponse = response as MiningBoxUpgradeRewardResponse;
            if (miningBoxUpgradeRewardResponse != null)
            {
                MiningDataModule dataModule = GameApp.Data.GetDataModule(DataName.MiningDataModule);
                dataModule.UpdateMiningInfo(miningBoxUpgradeRewardResponse.MiningInfoDto);
                dataModule.CacheTicket();
                dataModule.ClearCachePos();
                if (miningBoxUpgradeRewardResponse.Code == 0)
                {
                    Action<bool, MiningBoxUpgradeRewardResponse> callback2 = callback;
                    if (callback2 != null)
                    {
                        callback2(true, miningBoxUpgradeRewardResponse);
                    }
                    GameApp.SDK.Analyze.Track_MiningMine_Capy(miningBoxUpgradeRewardResponse.MiningInfoDto.TreasureUpGradeInfo, miningBoxUpgradeRewardResponse.CommonData.Reward);
                    return;
                }
                Action<bool, MiningBoxUpgradeRewardResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, miningBoxUpgradeRewardResponse);
            }
        }, true, false, string.Empty, true);
    }

    public static void DoMiningAdGetItemRequest(Action<bool, MiningAdGetItemResponse> callback)
    {
        MiningAdGetItemRequest miningAdGetItemRequest = new MiningAdGetItemRequest();
        miningAdGetItemRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(miningAdGetItemRequest, delegate (IMessage response)
        {
            MiningAdGetItemResponse miningAdGetItemResponse = response as MiningAdGetItemResponse;
            if (miningAdGetItemResponse != null)
            {
                GameApp.Data.GetDataModule(DataName.AdDataModule).UpdateAdData(miningAdGetItemResponse.AdData);
                GameApp.Data.GetDataModule(DataName.MiningDataModule).CacheTicket();
                if (miningAdGetItemResponse.Code == 0)
                {
                    Action<bool, MiningAdGetItemResponse> callback2 = callback;
                    if (callback2 == null)
                    {
                        return;
                    }
                    callback2(true, miningAdGetItemResponse);
                    return;
                }
                else
                {
                    Action<bool, MiningAdGetItemResponse> callback3 = callback;
                    if (callback3 == null)
                    {
                        return;
                    }
                    callback3(false, miningAdGetItemResponse);
                }
            }
        }, true, false, string.Empty, true);
    }
}
