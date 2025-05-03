
public class Login
{
    public static void UserLoginRequest(Action<bool, UserLoginResponse> callback)
    {
        UserLoginRequest userLoginRequest = new UserLoginRequest();
        userLoginRequest.CommonParams = NetworkUtils.GetCommonParams();
        userLoginRequest.CommonParams.ServerId = SelectServerDataModule.GetJumpServerId(GameApp.NetWork.m_account, GameApp.NetWork.m_account2, GameApp.NetWork.m_deviceID);
        userLoginRequest.ChannelId = 1U;
        userLoginRequest.AccountId2 = GameApp.NetWork.m_account2;
        GameApp.NetWork.Send(userLoginRequest, delegate (IMessage response)
        {
            UserLoginResponse userLoginResponse = response as UserLoginResponse;
            if (userLoginResponse != null && userLoginResponse.Code == 0)
            {
                SelectServerDataModule.ClearJumpServerData();
                NetworkUtils.m_paramToken = userLoginResponse.AccessToken;
                GameApp.NetWork.m_userID = userLoginResponse.UserId.ToString();
                GameApp.NetWork.m_serverID = userLoginResponse.ServerId;
                Action<bool, UserLoginResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, userLoginResponse);
                return;
            }
            else
            {
                HLog.LogError("NetworkUtils.Login.DoUserLoginResponse.resp == null");
                Action<bool, UserLoginResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, userLoginResponse);
                return;
            }
        }, false, false, string.Empty, false);
    }

    public static void OnLoginGetActivityCommonInfo(Action<int, int> onLoginFailure = null)
    {
        NetworkUtils.ActivityCommon.ActivityGetListRequest(false, delegate (bool isOk, int code)
        {
            if (!isOk)
            {
                Action<int, int> onLoginFailure2 = onLoginFailure;
                if (onLoginFailure2 == null)
                {
                    return;
                }
                onLoginFailure2(11401, code);
            }
        });
    }

    public static void OnLoginGetActivitySlotTrainInfo(Action<int, int> onLoginFailure = null)
    {
        NetworkUtils.ActivitySlotTrain.RequestTurnTableGetInfo(false, delegate (bool isOk, int code)
        {
            if (!isOk)
            {
                Action<int, int> onLoginFailure2 = onLoginFailure;
                if (onLoginFailure2 == null)
                {
                    return;
                }
                onLoginFailure2(11701, code);
            }
        });
    }

    public static void OnLoginGetWorldBossInfo(Action<int, int> onLoginFailure = null)
    {
        NetworkUtils.WorldBoss.DoGetWorldBossInfo(false, delegate (bool isOk, int code)
        {
            if (!isOk)
            {
                Action<int, int> onLoginFailure2 = onLoginFailure;
                if (onLoginFailure2 == null)
                {
                    return;
                }
                onLoginFailure2(10415, code);
            }
        });
    }
}
