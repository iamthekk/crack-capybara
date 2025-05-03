
public class User
{
    public static void DoUserGetPlayerInfoRequest(List<long> userIDs, Action<bool, UserGetOtherPlayerInfoResponse> callback)
    {
        UserGetOtherPlayerInfoRequest userGetOtherPlayerInfoRequest = new UserGetOtherPlayerInfoRequest();
        userGetOtherPlayerInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
        userGetOtherPlayerInfoRequest.OtherUserIds.AddRange(userIDs);
        userGetOtherPlayerInfoRequest.NeedAttrDetail = true;
        GameApp.NetWork.Send(userGetOtherPlayerInfoRequest, delegate (IMessage response)
        {
            UserGetOtherPlayerInfoResponse userGetOtherPlayerInfoResponse = response as UserGetOtherPlayerInfoResponse;
            if (userGetOtherPlayerInfoResponse != null && userGetOtherPlayerInfoResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.PlayerInformationDataModule).AddOrUpdateCachePlayerInfo(userGetOtherPlayerInfoResponse.PlayerInfos.ToList<PlayerInfoDto>());
                Action<bool, UserGetOtherPlayerInfoResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, userGetOtherPlayerInfoResponse);
                return;
            }
            else
            {
                Action<bool, UserGetOtherPlayerInfoResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, userGetOtherPlayerInfoResponse);
                return;
            }
        }, false, false, string.Empty, true);
    }

    public static void DoUserGetCityInfoRequest(long userID, Action<bool, UserGetCityInfoResponse> callback)
    {
        UserGetCityInfoRequest userGetCityInfoRequest = new UserGetCityInfoRequest();
        userGetCityInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
        userGetCityInfoRequest.UserId = userID;
        GameApp.NetWork.Send(userGetCityInfoRequest, delegate (IMessage response)
        {
            UserGetCityInfoResponse userGetCityInfoResponse = response as UserGetCityInfoResponse;
            if (userGetCityInfoResponse != null && userGetCityInfoResponse.Code == 0)
            {
                Action<bool, UserGetCityInfoResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, userGetCityInfoResponse);
                return;
            }
            else
            {
                Action<bool, UserGetCityInfoResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, userGetCityInfoResponse);
                return;
            }
        }, false, false, string.Empty, true);
    }

    public static void DoUserHeartbeatSyncRequest(Action<bool, UserHeartbeatSyncResponse> callback)
    {
        UserHeartbeatSyncRequest userHeartbeatSyncRequest = new UserHeartbeatSyncRequest();
        userHeartbeatSyncRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(userHeartbeatSyncRequest, delegate (IMessage response)
        {
            UserHeartbeatSyncResponse userHeartbeatSyncResponse = response as UserHeartbeatSyncResponse;
            if (userHeartbeatSyncResponse != null && userHeartbeatSyncResponse.Code == 0)
            {
                DxxTools.Time.SyncServerTimestamp((long)userHeartbeatSyncResponse.Time);
                if (userHeartbeatSyncResponse.SetCloseFunction)
                {
                    FunctionDataModule dataModule = GameApp.Data.GetDataModule(DataName.FunctionDataModule);
                    if (dataModule != null && userHeartbeatSyncResponse.CloseFunctionId != null)
                    {
                        dataModule.CommonUpdateServerCloseFunction(userHeartbeatSyncResponse.CloseFunctionId);
                    }
                }
                Action<bool, UserHeartbeatSyncResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, userHeartbeatSyncResponse);
                return;
            }
            else
            {
                Action<bool, UserHeartbeatSyncResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, userHeartbeatSyncResponse);
                return;
            }
        }, false, false, string.Empty, true);
    }

    public static void DoUserGetBattleReportRequest(ulong reportid, Action<bool, UserGetBattleReportResponse> callback)
    {
        UserGetBattleReportRequest userGetBattleReportRequest = new UserGetBattleReportRequest();
        userGetBattleReportRequest.CommonParams = NetworkUtils.GetCommonParams();
        userGetBattleReportRequest.ReportId = reportid;
        GameApp.NetWork.Send(userGetBattleReportRequest, delegate (IMessage response)
        {
            UserGetBattleReportResponse userGetBattleReportResponse = response as UserGetBattleReportResponse;
            if (userGetBattleReportResponse != null && userGetBattleReportResponse.Code == 0)
            {
                Action<bool, UserGetBattleReportResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, userGetBattleReportResponse);
                return;
            }
            else
            {
                Action<bool, UserGetBattleReportResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, userGetBattleReportResponse);
                return;
            }
        }, false, false, string.Empty, true);
    }

    public static void DoUserUpdateGuideMaskRequest(ulong guideMask, Action<bool, UserUpdateGuideMaskResponse> callback)
    {
        UserUpdateGuideMaskRequest userUpdateGuideMaskRequest = new UserUpdateGuideMaskRequest();
        userUpdateGuideMaskRequest.CommonParams = NetworkUtils.GetCommonParams();
        userUpdateGuideMaskRequest.GuideMask = guideMask;
        GameApp.NetWork.Send(userUpdateGuideMaskRequest, delegate (IMessage response)
        {
            UserUpdateGuideMaskResponse userUpdateGuideMaskResponse = response as UserUpdateGuideMaskResponse;
            if (userUpdateGuideMaskResponse != null && userUpdateGuideMaskResponse.Code == 0)
            {
                Action<bool, UserUpdateGuideMaskResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, userUpdateGuideMaskResponse);
                return;
            }
            else
            {
                Action<bool, UserUpdateGuideMaskResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, userUpdateGuideMaskResponse);
                return;
            }
        }, false, false, string.Empty, true);
    }

    public static void bindHabbyId(string email, string authCode, string tgaDeviceId, string tgaDistinctId, string language, Action<bool, UserHabbyMailBindResponse> callback)
    {
        UserHabbyMailBindRequest userHabbyMailBindRequest = new UserHabbyMailBindRequest();
        userHabbyMailBindRequest.CommonParams = NetworkUtils.GetCommonParams();
        userHabbyMailBindRequest.EmailDress = email;
        userHabbyMailBindRequest.BindParams.Add("authCode", authCode);
        userHabbyMailBindRequest.BindParams.Add("tgaDeviceId", tgaDeviceId);
        userHabbyMailBindRequest.BindParams.Add("tgaDistinctId", tgaDistinctId);
        userHabbyMailBindRequest.BindParams.Add("language", language);
        GameApp.NetWork.Send(userHabbyMailBindRequest, delegate (IMessage response)
        {
            UserHabbyMailBindResponse userHabbyMailBindResponse = response as UserHabbyMailBindResponse;
            if (userHabbyMailBindResponse != null && userHabbyMailBindResponse.Code == 0)
            {
                Action<bool, UserHabbyMailBindResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, userHabbyMailBindResponse);
                return;
            }
            else
            {
                Action<bool, UserHabbyMailBindResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, userHabbyMailBindResponse);
                return;
            }
        }, false, false, string.Empty, true);
    }

    public static void bindHabbyIdReward(Action<bool, UserHabbyMailRewardResponse> callback)
    {
        UserHabbyMailRewardRequest userHabbyMailRewardRequest = new UserHabbyMailRewardRequest();
        userHabbyMailRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(userHabbyMailRewardRequest, delegate (IMessage response)
        {
            UserHabbyMailRewardResponse userHabbyMailRewardResponse = response as UserHabbyMailRewardResponse;
            if (userHabbyMailRewardResponse != null && userHabbyMailRewardResponse.Code == 0)
            {
                Action<bool, UserHabbyMailRewardResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, userHabbyMailRewardResponse);
                return;
            }
            else
            {
                Action<bool, UserHabbyMailRewardResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, userHabbyMailRewardResponse);
                return;
            }
        }, false, false, string.Empty, true);
    }
}

