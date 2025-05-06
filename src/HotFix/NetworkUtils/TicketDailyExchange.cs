
public class TicketDailyExchange
{
    public static void ADGetRewardRequest(int adType, int adID, Action<bool, ADGetRewardResponse> callBack)
    {
        ADGetRewardRequest adgetRewardRequest = new ADGetRewardRequest
        {
            CommonParams = NetworkUtils.GetCommonParams(),
            AdType = adType
        };
        GameApp.NetWork.Send(adgetRewardRequest, delegate (IMessage response)
        {
            ADGetRewardResponse adgetRewardResponse = response as ADGetRewardResponse;
            if (adgetRewardResponse != null && adgetRewardResponse.Code == 0)
            {
                if (adgetRewardResponse.CommonData != null)
                {
                    if (adgetRewardResponse.CommonData.AdData != null)
                    {
                        GameApp.Data.GetDataModule(DataName.AdDataModule).UpdateAdData(adgetRewardResponse.CommonData.AdData);
                    }
                    if (adgetRewardResponse.CommonData != null && adgetRewardResponse.CommonData.Reward != null && adgetRewardResponse.CommonData.Reward.Count > 0)
                    {
                        DxxTools.UI.OpenRewardCommon(adgetRewardResponse.CommonData.Reward, null, true);
                    }
                }
                GameApp.Data.GetDataModule(DataName.TicketDailyExchangeDataModule).SetFreeAdLifeReFreshTime(UserTicketKind.UserLife, adgetRewardResponse.FreeAdLifeRefreshtime);
                Action<bool, ADGetRewardResponse> callBack2 = callBack;
                if (callBack2 != null)
                {
                    callBack2(true, adgetRewardResponse);
                }
                if (adType == 1)
                {
                    GameApp.SDK.Analyze.Track_AD(GameTGATools.ADSourceName(adID), "REWARD ", "", adgetRewardResponse.CommonData.Reward, null);
                    return;
                }
            }
            else
            {
                Action<bool, ADGetRewardResponse> callBack3 = callBack;
                if (callBack3 == null)
                {
                    return;
                }
                callBack3(false, adgetRewardResponse);
            }
        }, true, false, string.Empty, true);
    }
}
