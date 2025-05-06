
public class Sociality
{
    public static void DoSocialPowerRankRequest(int page, Action<bool, SocialPowerRankResponse> callback)
    {
        SocialPowerRankRequest socialPowerRankRequest = new SocialPowerRankRequest();
        socialPowerRankRequest.CommonParams = NetworkUtils.GetCommonParams();
        socialPowerRankRequest.Page = page;
        GameApp.NetWork.Send(socialPowerRankRequest, delegate (IMessage response)
        {
            SocialPowerRankResponse socialPowerRankResponse = response as SocialPowerRankResponse;
            if (socialPowerRankResponse != null && socialPowerRankResponse.Code == 0)
            {
                Action<bool, SocialPowerRankResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, socialPowerRankResponse);
                return;
            }
            else
            {
                Action<bool, SocialPowerRankResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, socialPowerRankResponse);
                return;
            }
        }, false, false, string.Empty, true);
    }

    public static void DoInteractListRequest(Action<bool, InteractListResponse> callback)
    {
        InteractListRequest interactListRequest = new InteractListRequest();
        interactListRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(interactListRequest, delegate (IMessage response)
        {
            InteractListResponse interactListResponse = response as InteractListResponse;
            if (interactListResponse != null && interactListResponse.Code == 0)
            {
                Action<bool, InteractListResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, interactListResponse);
                }
                RedPointController.Instance.ReCalc("Main.Sociality", true);
                return;
            }
            Action<bool, InteractListResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, interactListResponse);
        }, false, false, string.Empty, true);
    }

    public static void DoInteractDetailRequest(long rowID, Action<bool, InteractDetailResponse> callback)
    {
        InteractDetailRequest interactDetailRequest = new InteractDetailRequest();
        interactDetailRequest.CommonParams = NetworkUtils.GetCommonParams();
        interactDetailRequest.RowId = rowID;
        GameApp.NetWork.Send(interactDetailRequest, delegate (IMessage response)
        {
            InteractDetailResponse interactDetailResponse = response as InteractDetailResponse;
            if (interactDetailResponse != null && interactDetailResponse.Code == 0)
            {
                Action<bool, InteractDetailResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, interactDetailResponse);
                }
                RedPointController.Instance.ReCalc("Main.Sociality", true);
                return;
            }
            Action<bool, InteractDetailResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, interactDetailResponse);
        }, false, false, string.Empty, true);
    }
}
