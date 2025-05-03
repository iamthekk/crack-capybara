
public class SiGnIn
{
    public static void SignInGetInfoRequest(Action<bool, SignInGetInfoResponse> callback)
    {
        SignInGetInfoRequest signInGetInfoRequest = new SignInGetInfoRequest();
        signInGetInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(signInGetInfoRequest, delegate (IMessage response)
        {
            SignInGetInfoResponse signInGetInfoResponse = response as SignInGetInfoResponse;
            if (signInGetInfoResponse != null && signInGetInfoResponse.Code == 0)
            {
                Action<bool, SignInGetInfoResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, signInGetInfoResponse);
                return;
            }
            else
            {
                Action<bool, SignInGetInfoResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, signInGetInfoResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void SignInDoSignRequest(Action<bool, SignInDoSignResponse> callback)
    {
        SignInDoSignRequest signInDoSignRequest = new SignInDoSignRequest();
        signInDoSignRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(signInDoSignRequest, delegate (IMessage response)
        {
            SignInDoSignResponse signInDoSignResponse = response as SignInDoSignResponse;
            if (signInDoSignResponse != null && signInDoSignResponse.Code == 0)
            {
                Action<bool, SignInDoSignResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, signInDoSignResponse);
                return;
            }
            else
            {
                Action<bool, SignInDoSignResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, signInDoSignResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }
}
