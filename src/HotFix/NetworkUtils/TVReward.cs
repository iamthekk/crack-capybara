
public class TVReward
{
    public static void GetTVInfoListRequest(bool isLoginRequest = false, bool isShowMask = true, bool showError = true)
    {
        if (isLoginRequest)
        {
            isShowMask = false;
            showError = false;
        }
        GMVideoListRequest gmvideoListRequest = new GMVideoListRequest
        {
            CommonParams = NetworkUtils.GetCommonParams()
        };
        GameApp.NetWork.Send(gmvideoListRequest, delegate (IMessage response)
        {
            GMVideoListResponse gmvideoListResponse = response as GMVideoListResponse;
            if (gmvideoListResponse != null && gmvideoListResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.TVRewardDataModule).OnNetResponse_TVInfoList(gmvideoListResponse.GmVideoDtos, isLoginRequest);
            }
        }, isShowMask, false, string.Empty, showError);
    }
}
