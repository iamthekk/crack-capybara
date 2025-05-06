
public class Conquer
{
    public static void DoConquerListRequest(long userID, Action<bool, ConquerListResponse> callback)
    {
        ConquerListRequest conquerListRequest = new ConquerListRequest();
        conquerListRequest.CommonParams = NetworkUtils.GetCommonParams();
        conquerListRequest.UserId = userID;
        GameApp.NetWork.Send(conquerListRequest, delegate (IMessage response)
        {
            ConquerListResponse conquerListResponse = response as ConquerListResponse;
            if (conquerListResponse != null && conquerListResponse.Code == 0)
            {
                Action<bool, ConquerListResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, conquerListResponse);
                return;
            }
            else
            {
                Action<bool, ConquerListResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, conquerListResponse);
                return;
            }
        }, false, false, string.Empty, true);
    }

    public static void DoConquerBattleRequest(long userID, Action<bool, ConquerBattleResponse> callback)
    {
        ConquerBattleRequest conquerBattleRequest = new ConquerBattleRequest();
        conquerBattleRequest.CommonParams = NetworkUtils.GetCommonParams();
        conquerBattleRequest.UserId = userID;
        GameApp.NetWork.Send(conquerBattleRequest, delegate (IMessage response)
        {
            ConquerBattleResponse conquerBattleResponse = response as ConquerBattleResponse;
            if (conquerBattleResponse != null && conquerBattleResponse.Code == 0)
            {
                EventArgsBattleConquerEnter instance = Singleton<EventArgsBattleConquerEnter>.Instance;
                instance.SetData(conquerBattleResponse.Record, false);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_BattleConquer_BattleConquerEnter, instance);
                EventArgsRefreshLordAddSlaveData instance2 = Singleton<EventArgsRefreshLordAddSlaveData>.Instance;
                instance2.Clear();
                instance2.SetData(conquerBattleResponse.Lord, (int)conquerBattleResponse.SlaveCount);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_GameLoginData_RefreshLordAddSlaveData, instance2);
                Action<bool, ConquerBattleResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, conquerBattleResponse);
                return;
            }
            else
            {
                Action<bool, ConquerBattleResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, conquerBattleResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoConquerRevoltRequest(long userID, Action<bool, ConquerRevoltResponse> callback)
    {
        ConquerRevoltRequest conquerRevoltRequest = new ConquerRevoltRequest();
        conquerRevoltRequest.CommonParams = NetworkUtils.GetCommonParams();
        conquerRevoltRequest.UserId = userID;
        GameApp.NetWork.Send(conquerRevoltRequest, delegate (IMessage response)
        {
            ConquerRevoltResponse conquerRevoltResponse = response as ConquerRevoltResponse;
            if (conquerRevoltResponse != null && conquerRevoltResponse.Code == 0)
            {
                EventArgsBattleConquerEnter instance = Singleton<EventArgsBattleConquerEnter>.Instance;
                instance.SetData(conquerRevoltResponse.Record, false);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_BattleConquer_BattleConquerEnter, instance);
                EventArgsRefreshLordAddSlaveData instance2 = Singleton<EventArgsRefreshLordAddSlaveData>.Instance;
                instance2.Clear();
                instance2.SetData(conquerRevoltResponse.Lord, (int)conquerRevoltResponse.SlaveCount);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_GameLoginData_RefreshLordAddSlaveData, instance2);
                Action<bool, ConquerRevoltResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, conquerRevoltResponse);
                return;
            }
            else
            {
                Action<bool, ConquerRevoltResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, conquerRevoltResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoConquerLootRequest(long userID, Action<bool, ConquerLootResponse> callback)
    {
        ConquerLootRequest conquerLootRequest = new ConquerLootRequest();
        conquerLootRequest.CommonParams = NetworkUtils.GetCommonParams();
        conquerLootRequest.UserId = userID;
        GameApp.NetWork.Send(conquerLootRequest, delegate (IMessage response)
        {
            ConquerLootResponse conquerLootResponse = response as ConquerLootResponse;
            if (conquerLootResponse != null && conquerLootResponse.Code == 0)
            {
                EventArgsBattleConquerEnter instance = Singleton<EventArgsBattleConquerEnter>.Instance;
                instance.SetData(conquerLootResponse.Record, false);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_BattleConquer_BattleConquerEnter, instance);
                EventArgsRefreshLordAddSlaveData instance2 = Singleton<EventArgsRefreshLordAddSlaveData>.Instance;
                instance2.Clear();
                instance2.SetData(conquerLootResponse.Lord, (int)conquerLootResponse.SlaveCount);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_GameLoginData_RefreshLordAddSlaveData, instance2);
                Action<bool, ConquerLootResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, conquerLootResponse);
                return;
            }
            else
            {
                Action<bool, ConquerLootResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, conquerLootResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoConquerPardonRequest(long userID, Action<bool, ConquerPardonResponse> callback)
    {
        ConquerPardonRequest conquerPardonRequest = new ConquerPardonRequest();
        conquerPardonRequest.CommonParams = NetworkUtils.GetCommonParams();
        conquerPardonRequest.UserId = userID;
        GameApp.NetWork.Send(conquerPardonRequest, delegate (IMessage response)
        {
            ConquerPardonResponse conquerPardonResponse = response as ConquerPardonResponse;
            if (conquerPardonResponse != null && conquerPardonResponse.Code == 0)
            {
                EventArgsRefreshLordAddSlaveData instance = Singleton<EventArgsRefreshLordAddSlaveData>.Instance;
                instance.Clear();
                instance.SetData(conquerPardonResponse.Lord, (int)conquerPardonResponse.SlaveCount);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_GameLoginData_RefreshLordAddSlaveData, instance);
                Action<bool, ConquerPardonResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, conquerPardonResponse);
                return;
            }
            else
            {
                Action<bool, ConquerPardonResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, conquerPardonResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }
}
