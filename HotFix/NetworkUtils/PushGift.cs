
public class PushGift
{
    public static void DoClosePushGift(int configId, Action<bool, IapPushRemoveResponse> callback)
    {
        IapPushRemoveRequest iapPushRemoveRequest = new IapPushRemoveRequest();
        iapPushRemoveRequest.ConfigId = configId;
        iapPushRemoveRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(iapPushRemoveRequest, delegate (IMessage response)
        {
            IapPushRemoveResponse iapPushRemoveResponse = response as IapPushRemoveResponse;
            if (iapPushRemoveResponse == null || iapPushRemoveResponse.Code != 0)
            {
                Action<bool, IapPushRemoveResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(false, iapPushRemoveResponse);
                }
                HLog.LogError(string.Format("发送关闭推送面板消息失败,ConfigId:{0}", configId));
                return;
            }
            Action<bool, IapPushRemoveResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(true, iapPushRemoveResponse);
        }, true, false, string.Empty, true);
    }

    public static void DoRefreshPushGiftData(Action<bool, GetIapPushDtoResponse> callBack)
    {
        GetIapPushDtoRequest getIapPushDtoRequest = new GetIapPushDtoRequest();
        getIapPushDtoRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(getIapPushDtoRequest, delegate (IMessage response)
        {
            GetIapPushDtoResponse getIapPushDtoResponse = response as GetIapPushDtoResponse;
            if (getIapPushDtoResponse == null || getIapPushDtoResponse.Code != 0)
            {
                Action<bool, GetIapPushDtoResponse> callBack2 = callBack;
                if (callBack2 != null)
                {
                    callBack2(false, getIapPushDtoResponse);
                }
                HLog.LogError("发送刷新推送面板消息失败");
                return;
            }
            Action<bool, GetIapPushDtoResponse> callBack3 = callBack;
            if (callBack3 == null)
            {
                return;
            }
            callBack3(true, getIapPushDtoResponse);
        }, true, false, string.Empty, true);
    }
}
