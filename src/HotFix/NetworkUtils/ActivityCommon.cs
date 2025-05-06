
public class ActivityCommon
{
    public static void ActivityGetListRequest(bool isShowMask = true, Action<bool, int> callback = null)
    {
        ActivityGetListRequest activityGetListRequest = new ActivityGetListRequest
        {
            CommonParams = NetworkUtils.GetCommonParams()
        };
        GameApp.NetWork.Send(activityGetListRequest, delegate (IMessage response)
        {
            ActivityGetListResponse activityGetListResponse = response as ActivityGetListResponse;
            if (activityGetListResponse != null)
            {
                if (activityGetListResponse.Code == 0)
                {
                    EventArgsActivityCommonData instance = Singleton<EventArgsActivityCommonData>.Instance;
                    instance.SetData(activityGetListResponse);
                    GameApp.Event.DispatchNow(null, 252, instance);
                    Action<bool, int> callback2 = callback;
                    if (callback2 == null)
                    {
                        return;
                    }
                    callback2(true, activityGetListResponse.Code);
                    return;
                }
                else
                {
                    Action<bool, int> callback3 = callback;
                    if (callback3 == null)
                    {
                        return;
                    }
                    callback3(false, activityGetListResponse.Code);
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
        }, isShowMask, false, string.Empty, false);
    }
}
