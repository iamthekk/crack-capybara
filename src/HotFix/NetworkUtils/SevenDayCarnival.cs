
public class SevenDayCarnival
{
    public static void RequestGetSevenDayInfo(Action<SevenDayTaskGetInfoResponse> onSuccess, Action<int> onError)
    {
        SevenDayTaskGetInfoRequest sevenDayTaskGetInfoRequest = new SevenDayTaskGetInfoRequest();
        sevenDayTaskGetInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(sevenDayTaskGetInfoRequest, delegate (IMessage response)
        {
            SevenDayTaskGetInfoResponse sevenDayTaskGetInfoResponse = response as SevenDayTaskGetInfoResponse;
            if (sevenDayTaskGetInfoResponse != null && sevenDayTaskGetInfoResponse.Code == 0)
            {
                Action<SevenDayTaskGetInfoResponse> onSuccess2 = onSuccess;
                if (onSuccess2 != null)
                {
                    onSuccess2(sevenDayTaskGetInfoResponse);
                }
            }
            else
            {
                Action<int> onError2 = onError;
                if (onError2 != null)
                {
                    onError2((sevenDayTaskGetInfoResponse != null) ? sevenDayTaskGetInfoResponse.Code : 0);
                }
            }
            RedPointController.Instance.ReCalc("Main.Carnival", true);
        }, false, false, string.Empty, true);
    }

    public static void RequestGetSevenDayTaskReward(int taskId, Action<SevenDayTaskRewardResponse> onSuccess, Action<int> onError)
    {
        SevenDayTaskRewardRequest sevenDayTaskRewardRequest = new SevenDayTaskRewardRequest
        {
            CommonParams = NetworkUtils.GetCommonParams(),
            TaskId = (uint)taskId
        };
        GameApp.NetWork.Send(sevenDayTaskRewardRequest, delegate (IMessage response)
        {
            SevenDayTaskRewardResponse sevenDayTaskRewardResponse = response as SevenDayTaskRewardResponse;
            if (sevenDayTaskRewardResponse != null && sevenDayTaskRewardResponse.Code == 0)
            {
                Action<SevenDayTaskRewardResponse> onSuccess2 = onSuccess;
                if (onSuccess2 != null)
                {
                    onSuccess2(sevenDayTaskRewardResponse);
                }
            }
            else
            {
                Action<int> onError2 = onError;
                if (onError2 != null)
                {
                    onError2((sevenDayTaskRewardResponse != null) ? sevenDayTaskRewardResponse.Code : 0);
                }
            }
            RedPointController.Instance.ReCalc("Main.Carnival", true);
        }, true, false, string.Empty, true);
    }

    public static void RequestGetCarnivalActiveReward(int configId, int selectedRewardIdx, Action<SevenDayTaskActiveRewardResponse> onSuccess, Action<int> onError)
    {
        SevenDayTaskActiveRewardRequest sevenDayTaskActiveRewardRequest = new SevenDayTaskActiveRewardRequest
        {
            CommonParams = NetworkUtils.GetCommonParams(),
            ConfigId = (uint)configId
        };
        sevenDayTaskActiveRewardRequest.SelectIdx = (uint)selectedRewardIdx;
        GameApp.NetWork.Send(sevenDayTaskActiveRewardRequest, delegate (IMessage response)
        {
            SevenDayTaskActiveRewardResponse sevenDayTaskActiveRewardResponse = response as SevenDayTaskActiveRewardResponse;
            if (sevenDayTaskActiveRewardResponse != null && sevenDayTaskActiveRewardResponse.Code == 0)
            {
                Action<SevenDayTaskActiveRewardResponse> onSuccess2 = onSuccess;
                if (onSuccess2 != null)
                {
                    onSuccess2(sevenDayTaskActiveRewardResponse);
                }
            }
            else
            {
                Action<int> onError2 = onError;
                if (onError2 != null)
                {
                    onError2((sevenDayTaskActiveRewardResponse != null) ? sevenDayTaskActiveRewardResponse.Code : 0);
                }
            }
            RedPointController.Instance.ReCalc("Main.Carnival", true);
        }, true, false, string.Empty, true);
    }

    public static void RequestSevenDayFreeReward(int configId, Action<SevenDayFreeRewardResponse> onSuccess, Action<int> onError)
    {
        SevenDayFreeRewardRequest sevenDayFreeRewardRequest = new SevenDayFreeRewardRequest
        {
            CommonParams = NetworkUtils.GetCommonParams(),
            ConfigId = (uint)configId
        };
        GameApp.NetWork.Send(sevenDayFreeRewardRequest, delegate (IMessage response)
        {
            SevenDayFreeRewardResponse sevenDayFreeRewardResponse = response as SevenDayFreeRewardResponse;
            if (sevenDayFreeRewardResponse != null && sevenDayFreeRewardResponse.Code == 0)
            {
                Action<SevenDayFreeRewardResponse> onSuccess2 = onSuccess;
                if (onSuccess2 != null)
                {
                    onSuccess2(sevenDayFreeRewardResponse);
                }
            }
            else
            {
                Action<int> onError2 = onError;
                if (onError2 != null)
                {
                    onError2((sevenDayFreeRewardResponse != null) ? sevenDayFreeRewardResponse.Code : 0);
                }
            }
            RedPointController.Instance.ReCalc("Main.Carnival", true);
        }, true, false, string.Empty, true);
    }
}
