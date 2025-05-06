
public class Mount
{
    public static void DoMountUpgradeRequest(Action<bool, MountUpgradeResponse> callback)
    {
        MountUpgradeRequest mountUpgradeRequest = new MountUpgradeRequest();
        mountUpgradeRequest.CommonParams = NetworkUtils.GetCommonParams();
        MountInfo mountInfo = GameApp.Data.GetDataModule(DataName.MountDataModule).MountInfo;
        int preStage = (int)mountInfo.Stage;
        int preLevel = (int)mountInfo.Level;
        GameApp.NetWork.Send(mountUpgradeRequest, delegate (IMessage response)
        {
            MountUpgradeResponse mountUpgradeResponse = response as MountUpgradeResponse;
            if (mountUpgradeResponse != null && mountUpgradeResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.MountDataModule).UpdateMountInfo(mountUpgradeResponse.MountInfo);
                RedPointController.Instance.ReCalc("Equip.Mount.UpgradeTag", true);
                Action<bool, MountUpgradeResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, mountUpgradeResponse);
                }
                GameApp.SDK.Analyze.Track_MountLevel(mountUpgradeResponse.MountInfo, mountUpgradeResponse.CommonData.CostDto, preStage, preLevel);
                return;
            }
            Action<bool, MountUpgradeResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, mountUpgradeResponse);
        }, true, false, string.Empty, true);
    }

    public static void DoMountUpgradeAllRequest(Action<bool, MountUpgradeAllResponse> callback)
    {
        MountUpgradeAllRequest mountUpgradeAllRequest = new MountUpgradeAllRequest();
        mountUpgradeAllRequest.CommonParams = NetworkUtils.GetCommonParams();
        MountInfo mountInfo = GameApp.Data.GetDataModule(DataName.MountDataModule).MountInfo;
        int preStage = (int)mountInfo.Stage;
        int preLevel = (int)mountInfo.Level;
        GameApp.NetWork.Send(mountUpgradeAllRequest, delegate (IMessage response)
        {
            MountUpgradeAllResponse mountUpgradeAllResponse = response as MountUpgradeAllResponse;
            if (mountUpgradeAllResponse != null && mountUpgradeAllResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.MountDataModule).UpdateMountInfo(mountUpgradeAllResponse.MountInfo);
                RedPointController.Instance.ReCalc("Equip.Mount.UpgradeTag", true);
                Action<bool, MountUpgradeAllResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, mountUpgradeAllResponse);
                }
                GameApp.SDK.Analyze.Track_MountLevel(mountUpgradeAllResponse.MountInfo, mountUpgradeAllResponse.CommonData.CostDto, preStage, preLevel);
                return;
            }
            Action<bool, MountUpgradeAllResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, mountUpgradeAllResponse);
        }, true, false, string.Empty, true);
    }

    public static void DoMountItemStarRequest(int tableId, Action<bool, MountItemStarResponse> callback)
    {
        MountItemStarRequest mountItemStarRequest = new MountItemStarRequest();
        mountItemStarRequest.CommonParams = NetworkUtils.GetCommonParams();
        mountItemStarRequest.ConfigId = tableId;
        GameApp.NetWork.Send(mountItemStarRequest, delegate (IMessage response)
        {
            MountItemStarResponse mountItemStarResponse = response as MountItemStarResponse;
            if (mountItemStarResponse != null && mountItemStarResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.MountDataModule).UpdateMountItemDto(mountItemStarResponse.MountItemDto);
                RedPointController.Instance.ReCalc("Equip.Mount.AdvanceTag", true);
                Action<bool, MountItemStarResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, mountItemStarResponse);
                }
                GameApp.SDK.Analyze.Track_RareMountUpgrade(tableId, mountItemStarResponse.MountItemDto, mountItemStarResponse.CommonData.CostDto);
                return;
            }
            Action<bool, MountItemStarResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, mountItemStarResponse);
        }, true, false, string.Empty, true);
    }

    public static void DoMountApplySkillRequest(int advanceId, int optType, Action<bool, MountApplySkillResponse> callback)
    {
        MountApplySkillRequest mountApplySkillRequest = new MountApplySkillRequest();
        mountApplySkillRequest.CommonParams = NetworkUtils.GetCommonParams();
        mountApplySkillRequest.AdvanceId = advanceId;
        mountApplySkillRequest.OptType = optType;
        GameApp.NetWork.Send(mountApplySkillRequest, delegate (IMessage response)
        {
            MountApplySkillResponse mountApplySkillResponse = response as MountApplySkillResponse;
            if (mountApplySkillResponse != null && mountApplySkillResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.MountDataModule).UpdateMountInfo(mountApplySkillResponse.MountInfo);
                Action<bool, MountApplySkillResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, mountApplySkillResponse);
                return;
            }
            else
            {
                Action<bool, MountApplySkillResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, mountApplySkillResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoMountDressRequest(int type, int configId, int optType, Action<bool, MountDressResponse> callback)
    {
        MountDressRequest mountDressRequest = new MountDressRequest();
        mountDressRequest.CommonParams = NetworkUtils.GetCommonParams();
        mountDressRequest.ConfigType = type;
        mountDressRequest.ConfigId = configId;
        mountDressRequest.OptType = optType;
        if (optType != -1)
        {
            PlayerPrefsKeys.SetMountRideRed("1");
        }
        GameApp.NetWork.Send(mountDressRequest, delegate (IMessage response)
        {
            MountDressResponse mountDressResponse = response as MountDressResponse;
            if (mountDressResponse != null && mountDressResponse.Code == 0)
            {
                MountDataModule dataModule = GameApp.Data.GetDataModule(DataName.MountDataModule);
                uint configType = dataModule.MountInfo.ConfigType;
                uint configId2 = dataModule.MountInfo.ConfigId;
                dataModule.UpdateMountInfo(mountDressResponse.MountInfo);
                RedPointController.Instance.ReCalc("Equip.Mount.RideTag", true);
                if (configType != mountDressResponse.MountInfo.ConfigType || configId2 != mountDressResponse.MountInfo.ConfigId)
                {
                    GameApp.Event.DispatchNow(null, LocalMessageName.CC_UIMount_ChangeRide, null);
                }
                Action<bool, MountDressResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, mountDressResponse);
                }
                GameApp.SDK.Analyze.Track_EquipMount(mountDressResponse.MountInfo);
                return;
            }
            Action<bool, MountDressResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, mountDressResponse);
        }, true, false, string.Empty, true);
    }

    public static void DoMountUnlockRequest(int configId, Action<bool, MountUnlockResponse> callback)
    {
        MountUnlockRequest mountUnlockRequest = new MountUnlockRequest();
        mountUnlockRequest.CommonParams = NetworkUtils.GetCommonParams();
        mountUnlockRequest.ConfigId = configId;
        GameApp.NetWork.Send(mountUnlockRequest, delegate (IMessage response)
        {
            MountUnlockResponse mountUnlockResponse = response as MountUnlockResponse;
            if (mountUnlockResponse != null && mountUnlockResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.MountDataModule).UpdateMountItemDtos(mountUnlockResponse.MountItemDtos);
                RedPointController.Instance.ReCalc("Equip.Mount.AdvanceTag", true);
                Action<bool, MountUnlockResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, mountUnlockResponse);
                }
                GameApp.SDK.Analyze.Track_MountUnlock(configId);
                return;
            }
            Action<bool, MountUnlockResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, mountUnlockResponse);
        }, true, false, string.Empty, true);
    }
}
