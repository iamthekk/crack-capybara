
public class Relic
{
    public static void DoRelicActiveRequest(int id, Action<bool, RelicActiveResponse> callback = null)
    {
        RelicActiveRequest relicActiveRequest = new RelicActiveRequest();
        relicActiveRequest.CommonParams = NetworkUtils.GetCommonParams();
        relicActiveRequest.RelicId = id;
        GameApp.NetWork.Send(relicActiveRequest, delegate (IMessage response)
        {
            RelicActiveResponse relicActiveResponse = response as RelicActiveResponse;
            if (response != null && relicActiveResponse.Code == 0)
            {
                RedPointController.Instance.ReCalc("Equip.Relic", true);
                Action<bool, RelicActiveResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, relicActiveResponse);
                return;
            }
            else
            {
                Action<bool, RelicActiveResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, relicActiveResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoRelicStrengthRequest(int id, Action<bool, RelicStrengthResponse> callback = null)
    {
        RelicStrengthRequest relicStrengthRequest = new RelicStrengthRequest();
        relicStrengthRequest.CommonParams = NetworkUtils.GetCommonParams();
        relicStrengthRequest.RelicId = id;
        GameApp.NetWork.Send(relicStrengthRequest, delegate (IMessage response)
        {
            RelicStrengthResponse relicStrengthResponse = response as RelicStrengthResponse;
            if (response != null && relicStrengthResponse.Code == 0)
            {
                RedPointController.Instance.ReCalc("Equip.Relic", true);
                Action<bool, RelicStrengthResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, relicStrengthResponse);
                return;
            }
            else
            {
                Action<bool, RelicStrengthResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, relicStrengthResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoRelicStarRequest(int id, Action<bool, RelicStarResponse> callback = null)
    {
        RelicStarRequest relicStarRequest = new RelicStarRequest();
        relicStarRequest.CommonParams = NetworkUtils.GetCommonParams();
        relicStarRequest.RelicId = id;
        GameApp.NetWork.Send(relicStarRequest, delegate (IMessage response)
        {
            RelicStarResponse relicStarResponse = response as RelicStarResponse;
            if (response != null && relicStarResponse.Code == 0)
            {
                RedPointController.Instance.ReCalc("Equip.Relic", true);
                Action<bool, RelicStarResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, relicStarResponse);
                return;
            }
            else
            {
                Action<bool, RelicStarResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, relicStarResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }
}
