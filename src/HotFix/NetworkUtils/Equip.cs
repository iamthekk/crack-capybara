
public class Equip
{
    public static void EquipStrengthRequest(Action<bool, EquipStrengthResponse> callback, ulong rwoid, List<ulong> equipRowIds, Dictionary<uint, uint> useItems)
    {
        EquipStrengthRequest equipStrengthRequest = new EquipStrengthRequest();
        equipStrengthRequest.CommonParams = NetworkUtils.GetCommonParams();
        equipStrengthRequest.RowId = rwoid;
        equipStrengthRequest.UseItems.Add(useItems);
        equipStrengthRequest.EquipRowIds.AddRange(equipRowIds);
        GameApp.NetWork.Send(equipStrengthRequest, delegate (IMessage response)
        {
            EquipStrengthResponse equipStrengthResponse = response as EquipStrengthResponse;
            if (response != null && equipStrengthResponse.Code == 0)
            {
                GameApp.Event.DispatchNow(null, 145, null);
                Action<bool, EquipStrengthResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, equipStrengthResponse);
                return;
            }
            else
            {
                Action<bool, EquipStrengthResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, equipStrengthResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoEquipDress(List<ulong> rwoids, Action<bool, EquipDressResponse> callback = null)
    {
        EquipDressRequest equipDressRequest = new EquipDressRequest();
        equipDressRequest.CommonParams = NetworkUtils.GetCommonParams();
        equipDressRequest.RowIds.AddRange(rwoids);
        GameApp.NetWork.Send(equipDressRequest, delegate (IMessage response)
        {
            EquipDressResponse equipDressResponse = response as EquipDressResponse;
            if (response != null && equipDressResponse.Code == 0)
            {
                EventArgsRefreshEquipDressRowIds eventArgsRefreshEquipDressRowIds = new EventArgsRefreshEquipDressRowIds();
                eventArgsRefreshEquipDressRowIds.SetData(equipDressResponse.RowIds);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_EquipDataModule_RefreshEquipDressRowIds, eventArgsRefreshEquipDressRowIds);
                GameApp.Event.DispatchNow(null, 145, null);
                RedPointController.Instance.ReCalc("Equip.Hero", true);
                Action<bool, EquipDressResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, equipDressResponse);
                return;
            }
            else
            {
                Action<bool, EquipDressResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, equipDressResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoEquipUpgradeRequest(ulong rowID, uint count, Action<bool, EquipUpgradeResponse> callback = null)
    {
        EquipUpgradeRequest equipUpgradeRequest = new EquipUpgradeRequest();
        equipUpgradeRequest.CommonParams = NetworkUtils.GetCommonParams();
        equipUpgradeRequest.RowId = rowID;
        equipUpgradeRequest.Count = count;
        GameApp.NetWork.Send(equipUpgradeRequest, delegate (IMessage response)
        {
            EquipUpgradeResponse equipUpgradeResponse = response as EquipUpgradeResponse;
            if (response != null && equipUpgradeResponse.Code == 0)
            {
                RedPointController.Instance.ReCalc("Equip.Hero", true);
                GameApp.Event.DispatchNow(null, 145, null);
                Action<bool, EquipUpgradeResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, equipUpgradeResponse);
                }
                GameApp.SDK.Analyze.Track_EquipmentLevel(equipUpgradeResponse.CommonData.Equipment);
                return;
            }
            Action<bool, EquipUpgradeResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, equipUpgradeResponse);
        }, true, false, string.Empty, true);
    }

    public static void DoEquipEvolutionRequest(ulong rowID, Action<bool, EquipEvolutionResponse> callback = null)
    {
        EquipEvolutionRequest equipEvolutionRequest = new EquipEvolutionRequest();
        equipEvolutionRequest.CommonParams = NetworkUtils.GetCommonParams();
        equipEvolutionRequest.RowId = rowID;
        GameApp.NetWork.Send(equipEvolutionRequest, delegate (IMessage response)
        {
            EquipEvolutionResponse equipEvolutionResponse = response as EquipEvolutionResponse;
            if (equipEvolutionResponse == null)
            {
                HLog.LogError("EquipEvolutionResponse is null, please check");
                return;
            }
            if (response != null && equipEvolutionResponse.Code == 0)
            {
                RedPointController.Instance.ReCalc("Equip.Hero", true);
                GameApp.Event.DispatchNow(null, 145, null);
                Action<bool, EquipEvolutionResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, equipEvolutionResponse);
                }
                GameApp.SDK.Analyze.Track_EquipmentLevel(equipEvolutionResponse.CommonData.Equipment);
                return;
            }
            Action<bool, EquipEvolutionResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, equipEvolutionResponse);
        }, true, false, string.Empty, true);
    }

    public static void DoEquipComposeRequest(List<EquipComposeData> composeData, bool isAuto, Action<bool, EquipComposeResponse> callback = null)
    {
        EquipComposeRequest equipComposeRequest = new EquipComposeRequest();
        equipComposeRequest.CommonParams = NetworkUtils.GetCommonParams();
        equipComposeRequest.ComposeData.AddRange(composeData);
        GameApp.NetWork.Send(equipComposeRequest, delegate (IMessage response)
        {
            EquipComposeResponse equipComposeResponse = response as EquipComposeResponse;
            if (response != null && equipComposeResponse.Code == 0)
            {
                GameApp.SDK.Analyze.Track_EquipmentMerge(equipComposeResponse.CommonData.Equipment, equipComposeResponse.DelEquipRowId, isAuto);
                EventArgsRemoveEquipDatas instance = Singleton<EventArgsRemoveEquipDatas>.Instance;
                instance.m_datas = equipComposeResponse.DelEquipRowId;
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_EquipDataModule_RemoveEquipDatas, instance);
                List<ulong> equipDressRowIds = GameApp.Data.GetDataModule(DataName.EquipDataModule).m_equipDressRowIds;
                bool flag = false;
                if (equipDressRowIds.Count != equipComposeResponse.RowIds.Count)
                {
                    flag = true;
                }
                else
                {
                    for (int i = 0; i < equipDressRowIds.Count; i++)
                    {
                        if (equipDressRowIds[i] != equipComposeResponse.RowIds[i])
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if (flag)
                {
                    EventArgsRefreshEquipDressRowIds eventArgsRefreshEquipDressRowIds = new EventArgsRefreshEquipDressRowIds();
                    eventArgsRefreshEquipDressRowIds.SetData(equipComposeResponse.RowIds);
                    GameApp.Event.DispatchNow(null, LocalMessageName.CC_EquipDataModule_RefreshEquipDressRowIds, eventArgsRefreshEquipDressRowIds);
                }
                GameApp.Event.DispatchNow(null, 145, null);
                RedPointController.Instance.ReCalc("Equip.Hero", true);
                Action<bool, EquipComposeResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, equipComposeResponse);
                return;
            }
            else
            {
                Action<bool, EquipComposeResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, equipComposeResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoEquipResetRequest(ulong rowID, Action<bool, EquipLevelResetResponse> callback = null)
    {
        EquipLevelResetRequest equipLevelResetRequest = new EquipLevelResetRequest();
        equipLevelResetRequest.CommonParams = NetworkUtils.GetCommonParams();
        equipLevelResetRequest.RowIds.Add(rowID);
        GameApp.NetWork.Send(equipLevelResetRequest, delegate (IMessage response)
        {
            EquipLevelResetResponse equipLevelResetResponse = response as EquipLevelResetResponse;
            if (response != null && equipLevelResetResponse.Code == 0)
            {
                GameApp.Event.DispatchNow(null, 145, null);
                if (equipLevelResetResponse.CommonData.Reward.Count > 0)
                {
                    DxxTools.UI.OpenRewardCommon(equipLevelResetResponse.CommonData.Reward, null, true);
                }
                RedPointController.Instance.ReCalc("Equip.Hero", true);
                Action<bool, EquipLevelResetResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, equipLevelResetResponse);
                return;
            }
            else
            {
                Action<bool, EquipLevelResetResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, equipLevelResetResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoEquipDecomposeRequest(ulong rowID, Action<bool, EquipDecomposeResponse> callback = null)
    {
        EquipDecomposeRequest equipDecomposeRequest = new EquipDecomposeRequest();
        equipDecomposeRequest.CommonParams = NetworkUtils.GetCommonParams();
        equipDecomposeRequest.RowId = rowID;
        GameApp.NetWork.Send(equipDecomposeRequest, delegate (IMessage response)
        {
            EquipDecomposeResponse equipDecomposeResponse = response as EquipDecomposeResponse;
            if (response != null && equipDecomposeResponse.Code == 0)
            {
                GameApp.Event.DispatchNow(null, 145, null);
                if (equipDecomposeResponse.CommonData.Reward.Count > 0)
                {
                    DxxTools.UI.OpenRewardCommon(equipDecomposeResponse.CommonData.Reward, null, true);
                }
                RedPointController.Instance.ReCalc("Equip.Hero", true);
                Action<bool, EquipDecomposeResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, equipDecomposeResponse);
                return;
            }
            else
            {
                Action<bool, EquipDecomposeResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, equipDecomposeResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }
}
