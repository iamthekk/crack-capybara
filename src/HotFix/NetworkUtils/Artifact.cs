
public class Artifact
{
    public static void DoArtifactUpgradeRequest(Action<bool, ArtifactUpgradeResponse> callback)
    {
        ArtifactUpgradeRequest artifactUpgradeRequest = new ArtifactUpgradeRequest();
        artifactUpgradeRequest.CommonParams = NetworkUtils.GetCommonParams();
        ArtifactInfo artifactInfo = GameApp.Data.GetDataModule(DataName.ArtifactDataModule).ArtifactInfo;
        int preStage = (int)artifactInfo.Stage;
        int preLevel = (int)artifactInfo.Level;
        GameApp.NetWork.Send(artifactUpgradeRequest, delegate (IMessage response)
        {
            ArtifactUpgradeResponse artifactUpgradeResponse = response as ArtifactUpgradeResponse;
            if (artifactUpgradeResponse != null && artifactUpgradeResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.ArtifactDataModule).UpdateArtifactInfo(artifactUpgradeResponse.ArtifactInfo);
                RedPointController.Instance.ReCalc("Equip.Artifact.UpgradeTag", true);
                Action<bool, ArtifactUpgradeResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, artifactUpgradeResponse);
                }
                GameApp.SDK.Analyze.Track_LegendUpgrade(artifactUpgradeResponse.ArtifactInfo, artifactUpgradeResponse.CommonData.CostDto, preStage, preLevel);
                return;
            }
            Action<bool, ArtifactUpgradeResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, artifactUpgradeResponse);
        }, true, false, string.Empty, true);
    }

    public static void DoArtifactUpgradeAllRequest(Action<bool, ArtifactUpgradeAllResponse> callback)
    {
        ArtifactUpgradeAllRequest artifactUpgradeAllRequest = new ArtifactUpgradeAllRequest();
        artifactUpgradeAllRequest.CommonParams = NetworkUtils.GetCommonParams();
        ArtifactInfo artifactInfo = GameApp.Data.GetDataModule(DataName.ArtifactDataModule).ArtifactInfo;
        int preStage = (int)artifactInfo.Stage;
        int preLevel = (int)artifactInfo.Level;
        GameApp.NetWork.Send(artifactUpgradeAllRequest, delegate (IMessage response)
        {
            ArtifactUpgradeAllResponse artifactUpgradeAllResponse = response as ArtifactUpgradeAllResponse;
            if (artifactUpgradeAllResponse != null && artifactUpgradeAllResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.ArtifactDataModule).UpdateArtifactInfo(artifactUpgradeAllResponse.ArtifactInfo);
                RedPointController.Instance.ReCalc("Equip.Artifact.UpgradeTag", true);
                Action<bool, ArtifactUpgradeAllResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, artifactUpgradeAllResponse);
                }
                GameApp.SDK.Analyze.Track_LegendUpgrade(artifactUpgradeAllResponse.ArtifactInfo, artifactUpgradeAllResponse.CommonData.CostDto, preStage, preLevel);
                return;
            }
            Action<bool, ArtifactUpgradeAllResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, artifactUpgradeAllResponse);
        }, true, false, string.Empty, true);
    }

    public static void DoArtifactItemStarRequest(int tableId, Action<bool, ArtifactItemStarResponse> callback)
    {
        ArtifactItemStarRequest artifactItemStarRequest = new ArtifactItemStarRequest();
        artifactItemStarRequest.CommonParams = NetworkUtils.GetCommonParams();
        artifactItemStarRequest.ConfigId = tableId;
        GameApp.NetWork.Send(artifactItemStarRequest, delegate (IMessage response)
        {
            ArtifactItemStarResponse artifactItemStarResponse = response as ArtifactItemStarResponse;
            if (artifactItemStarResponse != null && artifactItemStarResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.ArtifactDataModule).UpdateArtifactItemDto(artifactItemStarResponse.ArtifactItemDto);
                RedPointController.Instance.ReCalc("Equip.Artifact.AdvanceTag", true);
                Action<bool, ArtifactItemStarResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, artifactItemStarResponse);
                }
                GameApp.SDK.Analyze.Track_RareLegendUpgrade(tableId, artifactItemStarResponse.ArtifactItemDto, artifactItemStarResponse.CommonData.CostDto);
                return;
            }
            Action<bool, ArtifactItemStarResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, artifactItemStarResponse);
        }, true, false, string.Empty, true);
    }

    public static void DoArtifactApplySkillRequest(int advanceId, int optType, Action<bool, ArtifactApplySkillResponse> callback)
    {
        ArtifactApplySkillRequest artifactApplySkillRequest = new ArtifactApplySkillRequest();
        artifactApplySkillRequest.CommonParams = NetworkUtils.GetCommonParams();
        artifactApplySkillRequest.AdvanceId = advanceId;
        artifactApplySkillRequest.OptType = optType;
        GameApp.NetWork.Send(artifactApplySkillRequest, delegate (IMessage response)
        {
            ArtifactApplySkillResponse artifactApplySkillResponse = response as ArtifactApplySkillResponse;
            if (artifactApplySkillResponse != null && artifactApplySkillResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.ArtifactDataModule).UpdateArtifactInfo(artifactApplySkillResponse.ArtifactInfo);
                Action<bool, ArtifactApplySkillResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, artifactApplySkillResponse);
                return;
            }
            else
            {
                Action<bool, ArtifactApplySkillResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, artifactApplySkillResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoArtifactUnlockRequest(int configId, Action<bool, ArtifactUnlockResponse> callback)
    {
        ArtifactUnlockRequest artifactUnlockRequest = new ArtifactUnlockRequest();
        artifactUnlockRequest.CommonParams = NetworkUtils.GetCommonParams();
        artifactUnlockRequest.ConfigId = configId;
        GameApp.NetWork.Send(artifactUnlockRequest, delegate (IMessage response)
        {
            ArtifactUnlockResponse artifactUnlockResponse = response as ArtifactUnlockResponse;
            if (artifactUnlockResponse != null && artifactUnlockResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.ArtifactDataModule).UpdateArtifactItemDtos(artifactUnlockResponse.ArtifactItemDto);
                RedPointController.Instance.ReCalc("Equip.Artifact.AdvanceTag", true);
                Action<bool, ArtifactUnlockResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, artifactUnlockResponse);
                }
                GameApp.SDK.Analyze.Track_UnlockRareLegend(configId);
                return;
            }
            Action<bool, ArtifactUnlockResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, artifactUnlockResponse);
        }, true, false, string.Empty, true);
    }
}
