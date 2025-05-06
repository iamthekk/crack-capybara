
public class UserHeartbeat
{
    public static void DoUserHeartbeatResponse(Action<UserHeartbeatResponse> callback)
    {
        UserHeartbeatRequest userHeartbeatRequest = new UserHeartbeatRequest();
        userHeartbeatRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(userHeartbeatRequest, delegate (IMessage response)
        {
            UserHeartbeatResponse userHeartbeatResponse = response as UserHeartbeatResponse;
            if (userHeartbeatResponse != null)
            {
                int code = userHeartbeatResponse.Code;
            }
            Action<UserHeartbeatResponse> callback2 = callback;
            if (callback2 == null)
            {
                return;
            }
            callback2(userHeartbeatResponse);
        }, false, false, string.Empty, true);
    }
}