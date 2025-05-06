
public class Collection
{
    public static void CollectionMergeRequest(uint rowId)
    {
        CollecComposeRequest collecComposeRequest = new CollecComposeRequest();
        collecComposeRequest.CommonParams = NetworkUtils.GetCommonParams();
        collecComposeRequest.RowId = (ulong)rowId;
        GameApp.NetWork.Send(collecComposeRequest, delegate (IMessage response)
        {
            CollecComposeResponse collecComposeResponse = response as CollecComposeResponse;
            if (collecComposeResponse != null && collecComposeResponse.Code == 0)
            {
                GameApp.Sound.PlayClip(630, 1f);
                GameApp.Data.GetDataModule(DataName.CollectionDataModule).TryCalcAttribute();
                EventArgsCollectionMerge eventArgsCollectionMerge = new EventArgsCollectionMerge();
                eventArgsCollectionMerge.SetData(collecComposeResponse.CollectionDto.ToList<CollectionDto>());
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_CollectionDataModule_CollectionMerge, eventArgsCollectionMerge);
                NetworkUtils.Collection.CheckSpecialCollectionChange(collecComposeResponse.CollectionDto);
                DxxTools.UI.OpenRewardCommon(collecComposeResponse.CommonData.Reward, delegate
                {
                    GameApp.Event.DispatchNow(null, 390, null);
                }, true);
                RedPointController.Instance.ReCalc("Equip.Collection.Main", true);
                RedPointController.Instance.ReCalc("Equip.Collection.StarUpgrade", true);
            }
        }, true, false, string.Empty, true);
    }

    public static void CollectionLevelUpRequest(uint rowId, Action<bool, CollecStrengthResponse> callback = null)
    {
        CollecStrengthRequest collecStrengthRequest = new CollecStrengthRequest();
        collecStrengthRequest.CommonParams = NetworkUtils.GetCommonParams();
        collecStrengthRequest.RowId = (ulong)rowId;
        GameApp.NetWork.Send(collecStrengthRequest, delegate (IMessage response)
        {
            CollecStrengthResponse collecStrengthResponse = response as CollecStrengthResponse;
            if (collecStrengthResponse != null && collecStrengthResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.CollectionDataModule).TryCalcAttribute();
                EventArgsCollectionLevelUp eventArgsCollectionLevelUp = new EventArgsCollectionLevelUp();
                eventArgsCollectionLevelUp.SetData(collecStrengthResponse.CollectionDto);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_CollectionDataModule_CollectionLevelUp, eventArgsCollectionLevelUp);
                Action<bool, CollecStrengthResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, collecStrengthResponse);
                }
                RedPointController.Instance.ReCalc("Equip.Collection.Main", true);
                return;
            }
            Action<bool, CollecStrengthResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, collecStrengthResponse);
        }, true, false, string.Empty, true);
    }

    public static void CollectionStarUpgradeRequest(uint rowId, Action<bool, CollecStarResponse> callback = null)
    {
        CollecStarRequest collecStarRequest = new CollecStarRequest();
        collecStarRequest.CommonParams = NetworkUtils.GetCommonParams();
        collecStarRequest.RowId = (ulong)rowId;
        GameApp.NetWork.Send(collecStarRequest, delegate (IMessage response)
        {
            CollecStarResponse collecStarResponse = response as CollecStarResponse;
            if (collecStarResponse != null && collecStarResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.CollectionDataModule).TryCalcAttribute();
                EventArgsCollectionStarUpgrade eventArgsCollectionStarUpgrade = new EventArgsCollectionStarUpgrade();
                eventArgsCollectionStarUpgrade.SetData(collecStarResponse.CollectionDto.ToList<CollectionDto>());
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_CollectionDataModule_CollectionStarUpgrade, eventArgsCollectionStarUpgrade);
                Action<bool, CollecStarResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, collecStarResponse);
                }
                RedPointController.Instance.ReCalc("Equip.Collection.Main", true);
                RedPointController.Instance.ReCalc("Equip.Collection.StarUpgrade", true);
                NetworkUtils.Collection.CheckSpecialCollectionChange(collecStarResponse.CollectionDto);
                return;
            }
            Action<bool, CollecStarResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, collecStarResponse);
        }, true, false, string.Empty, true);
    }

    private static void CheckSpecialCollectionChange(RepeatedField<CollectionDto> collectionDtos)
    {
        if (collectionDtos == null || collectionDtos.Count == 0)
        {
            return;
        }
        bool flag = false;
        for (int i = 0; i < collectionDtos.Count; i++)
        {
            CollectionDto collectionDto = collectionDtos[i];
            Collection_collection elementById = GameApp.Table.GetManager().GetCollection_collectionModelInstance().GetElementById((int)collectionDto.ConfigId);
            if (elementById != null && elementById.passiveType == 12)
            {
                flag = true;
                break;
            }
        }
        if (flag)
        {
            NetworkUtils.Chapter.DoGetHangUpInfoRequest(null);
        }
    }
}
