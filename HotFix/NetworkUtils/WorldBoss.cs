
public class WorldBoss
{
    public static void DoGetWorldBossInfo(bool isShowMask = true, Action<bool, int> callback = null)
    {
        WorldBossGetInfoRequest worldBossGetInfoRequest = new WorldBossGetInfoRequest
        {
            CommonParams = NetworkUtils.GetCommonParams()
        };
        GameApp.NetWork.Send(worldBossGetInfoRequest, delegate (IMessage response)
        {
            WorldBossGetInfoResponse worldBossGetInfoResponse = response as WorldBossGetInfoResponse;
            if (worldBossGetInfoResponse != null)
            {
                if (worldBossGetInfoResponse.Code == 0)
                {
                    WorldBossDataModule dataModule = GameApp.Data.GetDataModule(DataName.WorldBossDataModule);
                    dataModule.UpdateByServerInfo(worldBossGetInfoResponse.Info);
                    dataModule.DebugLog();
                    Action<bool, int> callback2 = callback;
                    if (callback2 == null)
                    {
                        return;
                    }
                    callback2(true, worldBossGetInfoResponse.Code);
                    return;
                }
                else
                {
                    Action<bool, int> callback3 = callback;
                    if (callback3 == null)
                    {
                        return;
                    }
                    callback3(false, worldBossGetInfoResponse.Code);
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

    public static void DoGetWorldBossBoxReward(int boxId, Action<bool> callBack = null)
    {
        WorldBossBoxRewardRequest worldBossBoxRewardRequest = new WorldBossBoxRewardRequest
        {
            CommonParams = NetworkUtils.GetCommonParams(),
            BoxRewardId = boxId
        };
        GameApp.NetWork.Send(worldBossBoxRewardRequest, delegate (IMessage response)
        {
            WorldBossBoxRewardResponse worldBossBoxRewardResponse = (WorldBossBoxRewardResponse)response;
            if (worldBossBoxRewardResponse != null && worldBossBoxRewardResponse.Code == 0)
            {
                WorldBossDataModule dataModule = GameApp.Data.GetDataModule(DataName.WorldBossDataModule);
                dataModule.UpdateRewardMaxClaimed(worldBossBoxRewardResponse);
                dataModule.DebugLog();
                Action<bool> callBack2 = callBack;
                if (callBack2 == null)
                {
                    return;
                }
                callBack2(true);
                return;
            }
            else
            {
                Action<bool> callBack3 = callBack;
                if (callBack3 == null)
                {
                    return;
                }
                callBack3(false);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoStartWorldBoss(Action<bool, StartWorldBossResponse> callback = null)
    {
        StartWorldBossRequest startWorldBossRequest = new StartWorldBossRequest();
        startWorldBossRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(startWorldBossRequest, delegate (IMessage response)
        {
            StartWorldBossResponse startWorldBossResponse = (StartWorldBossResponse)response;
            if (startWorldBossResponse != null && startWorldBossResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.WorldBossDataModule).UpdateReadyBattleInfo(startWorldBossResponse);
                Action<bool, StartWorldBossResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, startWorldBossResponse);
                return;
            }
            else
            {
                Action<bool, StartWorldBossResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, startWorldBossResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoEndWorldBoss(List<int> skills, Action<bool, EndWorldBossResponse> callback = null)
    {
        EndWorldBossRequest endWorldBossRequest = new EndWorldBossRequest();
        endWorldBossRequest.CommonParams = NetworkUtils.GetCommonParams();
        endWorldBossRequest.ClientVersion = Singleton<BattleServerVersionMgr>.Instance.GetVersion();
        for (int i = 0; i < skills.Count; i++)
        {
            endWorldBossRequest.RoundSkillList.Add(skills[i]);
        }
        GameApp.NetWork.Send(endWorldBossRequest, delegate (IMessage response)
        {
            EndWorldBossResponse endWorldBossResponse = (EndWorldBossResponse)response;
            if (endWorldBossResponse != null && endWorldBossResponse.Code == 0)
            {
                WorldBossDataModule dataModule = GameApp.Data.GetDataModule(DataName.WorldBossDataModule);
                dataModule.UpdateChallengeInfo(endWorldBossResponse);
                dataModule.DebugLog();
                Action<bool, EndWorldBossResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, endWorldBossResponse);
                return;
            }
            else
            {
                Action<bool, EndWorldBossResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, endWorldBossResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }
}

