
public class Chest
{
    public static void ChestUseRequest(ulong rowId, int count, Action<bool, ChestUseResponse> callback = null)
    {
        ChestUseRequest chestUseRequest = new ChestUseRequest();
        chestUseRequest.CommonParams = NetworkUtils.GetCommonParams();
        chestUseRequest.RowId = rowId;
        chestUseRequest.Count = (uint)count;
        GameApp.NetWork.Send(chestUseRequest, delegate (IMessage response)
        {
            ChestUseResponse chestUseResponse = response as ChestUseResponse;
            if (chestUseResponse != null && chestUseResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.ChestDataModule).UpdateCurrentScore();
                Action<bool, ChestUseResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, chestUseResponse);
                return;
            }
            else
            {
                Action<bool, ChestUseResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, chestUseResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void ChestRewardRequest(bool isGetAll, Action<bool, ChestRewardResponse> callback = null)
    {
        ChestRewardRequest chestRewardRequest = new ChestRewardRequest();
        chestRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
        chestRewardRequest.OptType = (isGetAll ? 1 : 0);
        ChestDataModule chestDataModule = GameApp.Data.GetDataModule(DataName.ChestDataModule);
        long preScore = chestDataModule.GetCurScore();
        GameApp.NetWork.Send(chestRewardRequest, delegate (IMessage response)
        {
            ChestRewardResponse chestRewardResponse = response as ChestRewardResponse;
            if (chestRewardResponse != null && chestRewardResponse.Code == 0)
            {
                chestDataModule.UpdateChestInfo(chestRewardResponse.ChestInfo);
                DxxTools.UI.OpenRewardCommon(chestRewardResponse.CommonData.Reward, delegate
                {
                    GameApp.Event.Dispatch(null, LocalMessageName.CC_Chest_ScoreRewardChange, null);
                    GameApp.Event.Dispatch(null, LocalMessageName.CC_Chest_ChestChange, null);
                }, true);
                Action<bool, ChestRewardResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, chestRewardResponse);
                }
                GameApp.SDK.Analyze.Track_TreasurePoint_Reward(isGetAll, preScore - chestDataModule.GetCurScore(), chestRewardResponse.CommonData.Reward);
                return;
            }
            Action<bool, ChestRewardResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, chestRewardResponse);
        }, true, false, string.Empty, true);
    }
}
