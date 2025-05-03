
public class CrossArena
{
    private static CrossArenaDataModule _DataModule
    {
        get
        {
            return GameApp.Data.GetDataModule(DataName.CrossArenaDataModule);
        }
    }

    public static void DoCrossArenaGetInfoRequest(Action<bool, CrossArenaGetInfoResponse> callback)
    {
        CrossArenaGetInfoRequest crossArenaGetInfoRequest = new CrossArenaGetInfoRequest();
        crossArenaGetInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(crossArenaGetInfoRequest, delegate (IMessage response)
        {
            CrossArenaGetInfoResponse crossArenaGetInfoResponse = response as CrossArenaGetInfoResponse;
            if (crossArenaGetInfoResponse == null || crossArenaGetInfoResponse.Code != 0)
            {
                Action<bool, CrossArenaGetInfoResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(false, crossArenaGetInfoResponse);
                }
                HLog.LogError(string.Format("跨服竞技场 获取基本数据 失败 {0}", (crossArenaGetInfoResponse != null) ? crossArenaGetInfoResponse.Code : (-999)));
                return;
            }
            EventArgsSetCrossArenaInfo eventArgsSetCrossArenaInfo = new EventArgsSetCrossArenaInfo();
            eventArgsSetCrossArenaInfo.SetData(crossArenaGetInfoResponse.Dan, crossArenaGetInfoResponse.Rank, crossArenaGetInfoResponse.Score, crossArenaGetInfoResponse.TeamCount, crossArenaGetInfoResponse.CurSeason);
            GameApp.Event.DispatchNow(null, LocalMessageName.CC_CrossArena_SetInfo, eventArgsSetCrossArenaInfo);
            Action<bool, CrossArenaGetInfoResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(true, crossArenaGetInfoResponse);
        }, false, false, string.Empty, true);
    }

    public static void DoCrossArenaChallengeListRequest(bool forcerefresh, Action<bool, CrossArenaChallengeListResponse> callback)
    {
        CrossArenaChallengeListRequest crossArenaChallengeListRequest = new CrossArenaChallengeListRequest();
        crossArenaChallengeListRequest.CommonParams = NetworkUtils.GetCommonParams();
        crossArenaChallengeListRequest.Refresh = forcerefresh;
        GameApp.NetWork.Send(crossArenaChallengeListRequest, delegate (IMessage response)
        {
            CrossArenaChallengeListResponse crossArenaChallengeListResponse = response as CrossArenaChallengeListResponse;
            if (crossArenaChallengeListResponse == null || crossArenaChallengeListResponse.Code != 0)
            {
                Action<bool, CrossArenaChallengeListResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(false, crossArenaChallengeListResponse);
                }
                HLog.LogError(string.Format("跨服竞技场-挑战列表 失败 {0}", (crossArenaChallengeListResponse != null) ? crossArenaChallengeListResponse.Code : (-999)));
                return;
            }
            EventArgsCrossArenaRefreshOppList eventArgsCrossArenaRefreshOppList = new EventArgsCrossArenaRefreshOppList();
            eventArgsCrossArenaRefreshOppList.Members = CrossArenaRankMember.ToListWithRank(crossArenaChallengeListResponse.OppList);
            eventArgsCrossArenaRefreshOppList.RefreshCount = (int)crossArenaChallengeListResponse.RefreshCount;
            GameApp.Event.DispatchNow(null, LocalMessageName.CC_CrossArena_RefreshChallengeList, eventArgsCrossArenaRefreshOppList);
            Action<bool, CrossArenaChallengeListResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(true, crossArenaChallengeListResponse);
        }, false, false, string.Empty, true);
    }

    public static void DoCrossArenaChallengeRequest(long userid, Action<bool, CrossArenaChallengeResponse> callback)
    {
        CrossArenaChallengeRequest crossArenaChallengeRequest = new CrossArenaChallengeRequest();
        crossArenaChallengeRequest.CommonParams = NetworkUtils.GetCommonParams();
        crossArenaChallengeRequest.UserId = userid;
        crossArenaChallengeRequest.ClientVersion = Singleton<BattleServerVersionMgr>.Instance.GetVersion();
        GameApp.NetWork.Send(crossArenaChallengeRequest, delegate (IMessage response)
        {
            CrossArenaChallengeResponse crossArenaChallengeResponse = response as CrossArenaChallengeResponse;
            if (crossArenaChallengeResponse == null || crossArenaChallengeResponse.Code != 0)
            {
                Action<bool, CrossArenaChallengeResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(false, crossArenaChallengeResponse);
                }
                HLog.LogError(string.Format("跨服竞技场-挑战 失败 {0}", (crossArenaChallengeResponse != null) ? crossArenaChallengeResponse.Code : (-999)));
                return;
            }
            GameApp.Data.GetDataModule(DataName.CrossArenaDataModule).RefreshOppListCount = 0;
            Action<bool, CrossArenaChallengeResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(true, crossArenaChallengeResponse);
        }, true, false, string.Empty, true);
    }

    public static void DoCrossArenaRankRequest(int page, Action<bool, CrossArenaRankResponse> callback)
    {
        CrossArenaRankRequest crossArenaRankRequest = new CrossArenaRankRequest();
        crossArenaRankRequest.CommonParams = NetworkUtils.GetCommonParams();
        crossArenaRankRequest.Page = (uint)page;
        GameApp.NetWork.Send(crossArenaRankRequest, delegate (IMessage response)
        {
            CrossArenaRankResponse crossArenaRankResponse = response as CrossArenaRankResponse;
            if (crossArenaRankResponse == null || crossArenaRankResponse.Code != 0)
            {
                Action<bool, CrossArenaRankResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(false, crossArenaRankResponse);
                }
                HLog.LogError(string.Format("跨服竞技场-排行榜 失败 {0}", (crossArenaRankResponse != null) ? crossArenaRankResponse.Code : (-999)));
                return;
            }
            if (crossArenaRankResponse.Rank.Count > 0)
            {
                long userId = GameApp.Data.GetDataModule(DataName.LoginDataModule).userId;
                CrossArenaDataModule dataModule = GameApp.Data.GetDataModule(DataName.CrossArenaDataModule);
                for (int i = 0; i < crossArenaRankResponse.Rank.Count; i++)
                {
                    if (crossArenaRankResponse.Rank[i] != null && crossArenaRankResponse.Rank[i].UserId == userId)
                    {
                        dataModule.UpdateMyRankInfo(crossArenaRankResponse.Rank[i]);
                    }
                }
            }
            Action<bool, CrossArenaRankResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(true, crossArenaRankResponse);
        }, false, false, string.Empty, true);
    }

    public static void DoCrossArenaRecordRequest(Action<bool, CrossArenaRecordResponse> callback)
    {
        CrossArenaRecordRequest crossArenaRecordRequest = new CrossArenaRecordRequest();
        crossArenaRecordRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(crossArenaRecordRequest, delegate (IMessage response)
        {
            CrossArenaRecordResponse crossArenaRecordResponse = response as CrossArenaRecordResponse;
            if (crossArenaRecordResponse == null || crossArenaRecordResponse.Code != 0)
            {
                Action<bool, CrossArenaRecordResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(false, crossArenaRecordResponse);
                }
                HLog.LogError(string.Format("跨服竞技场-对战记录 失败 {0}", (crossArenaRecordResponse != null) ? crossArenaRecordResponse.Code : (-999)));
                return;
            }
            Action<bool, CrossArenaRecordResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(true, crossArenaRecordResponse);
        }, false, false, string.Empty, true);
    }

    public static void DoCrossArenaEnterRequest(Action<bool, CrossArenaEnterResponse> callback)
    {
        CrossArenaEnterRequest crossArenaEnterRequest = new CrossArenaEnterRequest();
        crossArenaEnterRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(crossArenaEnterRequest, delegate (IMessage response)
        {
            CrossArenaEnterResponse crossArenaEnterResponse = response as CrossArenaEnterResponse;
            if (crossArenaEnterResponse == null || crossArenaEnterResponse.Code != 0)
            {
                Action<bool, CrossArenaEnterResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(false, crossArenaEnterResponse);
                }
                HLog.LogError(string.Format("跨服竞技场-进入 失败 {0}", (crossArenaEnterResponse != null) ? crossArenaEnterResponse.Code : (-999)));
                return;
            }
            EventArgsSetCrossArenaInfo eventArgsSetCrossArenaInfo = new EventArgsSetCrossArenaInfo();
            eventArgsSetCrossArenaInfo.SetData(crossArenaEnterResponse.Dan, crossArenaEnterResponse.Rank, crossArenaEnterResponse.Score, crossArenaEnterResponse.TeamCount, crossArenaEnterResponse.CurSeason, crossArenaEnterResponse.GroupId);
            GameApp.Event.DispatchNow(null, LocalMessageName.CC_CrossArena_SetInfo, eventArgsSetCrossArenaInfo);
            Action<bool, CrossArenaEnterResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(true, crossArenaEnterResponse);
        }, false, false, string.Empty, true);
    }
}
