
public class NewWorld
{
    public static void DoEnterRequest(Action<bool, EnterResponse> callback)
    {
        EnterRequest enterRequest = new EnterRequest();
        enterRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(enterRequest, delegate (IMessage response)
        {
            EnterResponse enterResponse = response as EnterResponse;
            if (enterResponse != null && enterResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.NewWorldDataModule).EnterNewWorld();
                Action<bool, EnterResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, enterResponse);
                return;
            }
            else
            {
                Action<bool, EnterResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, enterResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoLikeRequest(int index, Action<bool, LikeResponse> callback)
    {
        LikeRequest likeRequest = new LikeRequest();
        likeRequest.CommonParams = NetworkUtils.GetCommonParams();
        likeRequest.RankIndex = index;
        GameApp.NetWork.Send(likeRequest, delegate (IMessage response)
        {
            LikeResponse likeResponse = response as LikeResponse;
            if (likeResponse != null && likeResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.NewWorldDataModule).UpdateLikeInfo(likeResponse);
                Action<bool, LikeResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, likeResponse);
                return;
            }
            else
            {
                Action<bool, LikeResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, likeResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoNewWorldInfoRequest(Action<bool, NewWorldInfoResponse> callback)
    {
        NewWorldInfoRequest newWorldInfoRequest = new NewWorldInfoRequest();
        newWorldInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(newWorldInfoRequest, delegate (IMessage response)
        {
            NewWorldInfoResponse newWorldInfoResponse = response as NewWorldInfoResponse;
            if (newWorldInfoResponse != null && newWorldInfoResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.NewWorldDataModule).UpdateInfo(newWorldInfoResponse);
                Action<bool, NewWorldInfoResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, newWorldInfoResponse);
                return;
            }
            else
            {
                Action<bool, NewWorldInfoResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, newWorldInfoResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoNewWorldTaskRewardRequest(int tableId, Action<bool, NewWorldTaskRewardResponse> callback)
    {
        NewWorldTaskRewardRequest newWorldTaskRewardRequest = new NewWorldTaskRewardRequest();
        newWorldTaskRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
        newWorldTaskRewardRequest.ConfigId = tableId;
        GameApp.NetWork.Send(newWorldTaskRewardRequest, delegate (IMessage response)
        {
            NewWorldTaskRewardResponse newWorldTaskRewardResponse = response as NewWorldTaskRewardResponse;
            if (newWorldTaskRewardResponse != null && newWorldTaskRewardResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.NewWorldDataModule).UpdateTaskData(newWorldTaskRewardResponse);
                Action<bool, NewWorldTaskRewardResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, newWorldTaskRewardResponse);
                return;
            }
            else
            {
                Action<bool, NewWorldTaskRewardResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, newWorldTaskRewardResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }
}
