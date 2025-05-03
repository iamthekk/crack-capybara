
public class Pet
{
    public static void PetInfoRefreshRequest(Action<bool, UserRefDataResponse> callback)
    {
        UserRefDataRequest userRefDataRequest = new UserRefDataRequest();
        userRefDataRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(userRefDataRequest, delegate (IMessage response)
        {
            UserRefDataResponse userRefDataResponse = response as UserRefDataResponse;
            if (userRefDataResponse != null && userRefDataResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.PetDataModule).UpdatePetInfo(userRefDataResponse.PetInfo);
                RedPointController.Instance.ReCalc("Equip.Pet", true);
                GameApp.Event.DispatchNow(null, 153, null);
                Action<bool, UserRefDataResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, userRefDataResponse);
                return;
            }
            else
            {
                Action<bool, UserRefDataResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, null);
                return;
            }
        }, false, false, string.Empty, true);
    }

    public static void PetDrawRequest(int petBoxType, Action<PetDrawResponse> successCallback)
    {
        PetDrawRequest petDrawRequest = new PetDrawRequest();
        petDrawRequest.CommonParams = NetworkUtils.GetCommonParams();
        petDrawRequest.Type = petBoxType;
        GameApp.NetWork.Send(petDrawRequest, delegate (IMessage response)
        {
            PetDrawResponse petDrawResponse = response as PetDrawResponse;
            if (petDrawResponse != null && petDrawResponse.Code == 0)
            {
                PetDataModule dataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
                List<PetDto> list = new List<PetDto>();
                if (petDrawResponse.ShowPet != null)
                {
                    for (int i = 0; i < petDrawResponse.ShowPet.Count; i++)
                    {
                        PetDto petDto = petDrawResponse.ShowPet[i];
                        petDto.RowId = (ulong)((long)i + 1L);
                        if (GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)petDto.ConfigId)
                            .quality >= 6)
                        {
                            list.Add(petDto);
                        }
                    }
                    if (petDrawResponse.ShowPet.Count > 0 && list.Count == 0)
                    {
                        int num = 0;
                        int num2 = -1;
                        int num3 = -1;
                        for (int j = 0; j < petDrawResponse.ShowPet.Count; j++)
                        {
                            PetDto petDto2 = petDrawResponse.ShowPet[j];
                            Pet_pet elementById = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById((int)petDto2.ConfigId);
                            if (elementById != null && elementById.quality > num2 && elementById.id > num3)
                            {
                                num2 = elementById.quality;
                                num = j;
                                num3 = (int)petDto2.ConfigId;
                            }
                        }
                        list.Add(petDrawResponse.ShowPet[num]);
                    }
                }
                dataModule.UpdatePetDrawData(petDrawResponse);
                RedPointController.Instance.ReCalc("Equip.Pet", true);
                List<ItemData> list2 = new List<ItemData>();
                for (int k = 0; k < petDrawResponse.AddPet.Count; k++)
                {
                    PetDto petDto3 = petDrawResponse.AddPet[k];
                    ItemData itemData = new ItemData((int)petDto3.ConfigId, (long)((ulong)petDto3.PetCount));
                    list2.Add(itemData);
                }
                PetOpenEggViewModule.OpenData openData = new PetOpenEggViewModule.OpenData();
                openData.petBoxType = petBoxType;
                openData.petList = list;
                openData.rewardList = list2;
                openData.newPetRowIds = new List<ulong>();
                if (!GameApp.View.IsOpened(ViewName.PetOpenEggViewModule))
                {
                    GameApp.View.OpenView(ViewName.PetOpenEggViewModule, openData, 1, null, delegate
                    {
                        GameApp.Event.DispatchNow(null, LocalMessageName.CC_PetDataModule_RefreshPetDrawInfo, null);
                    });
                }
                else
                {
                    EventArgsDrawPetResultData eventArgsDrawPetResultData = new EventArgsDrawPetResultData();
                    eventArgsDrawPetResultData.openData = openData;
                    GameApp.Event.DispatchNow(null, LocalMessageName.CC_PetOpenEggViewModule_RefreshData, eventArgsDrawPetResultData);
                    GameApp.Event.DispatchNow(null, LocalMessageName.CC_PetDataModule_RefreshPetDrawInfo, null);
                }
                if (petBoxType == 11)
                {
                    GameApp.SDK.Analyze.Track_AD(GameTGATools.ADSourceName(8), "REWARD ", "", null, petDrawResponse.ShowPet);
                }
                Action<PetDrawResponse> successCallback2 = successCallback;
                if (successCallback2 == null)
                {
                    return;
                }
                successCallback2(petDrawResponse);
            }
        }, true, false, string.Empty, true);
    }

    public static void PetComposeRequest(ulong rowId, Action<bool, PetComposeResponse> callback)
    {
        PetComposeRequest petComposeRequest = new PetComposeRequest();
        petComposeRequest.CommonParams = NetworkUtils.GetCommonParams();
        petComposeRequest.RowId = rowId;
        GameApp.NetWork.Send(petComposeRequest, delegate (IMessage response)
        {
            PetComposeResponse petComposeResponse = response as PetComposeResponse;
            if (petComposeResponse != null && petComposeResponse.Code == 0)
            {
                EventArgsFragmentMergePet instance = Singleton<EventArgsFragmentMergePet>.Instance;
                instance.addPets = petComposeResponse.PetDto;
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_PetDataModule_FragmentMergePet, instance);
                RedPointController.Instance.ReCalc("Equip.Pet", true);
                Action<bool, PetComposeResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, petComposeResponse);
                }
                if (petComposeResponse.PetDto.Count > 0)
                {
                    PetStarUpgradeViewModule.OpenData openData = new PetStarUpgradeViewModule.OpenData();
                    openData.petData = GameApp.Data.GetDataModule(DataName.PetDataModule).GetPetData(petComposeResponse.PetDto[0].RowId);
                    GameApp.View.OpenView(ViewName.PetStarUpgradeViewModule, openData, 1, null, null);
                    return;
                }
            }
            else
            {
                Action<bool, PetComposeResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, petComposeResponse);
            }
        }, true, false, string.Empty, true);
    }

    public static void PetStarUpgradeRequest(ulong rowId, Action<bool, PetStarResponse> callback)
    {
        PetStarRequest petStarRequest = new PetStarRequest();
        petStarRequest.CommonParams = NetworkUtils.GetCommonParams();
        petStarRequest.RowId = rowId;
        GameApp.NetWork.Send(petStarRequest, delegate (IMessage response)
        {
            PetStarResponse petStarResponse = response as PetStarResponse;
            if (petStarResponse != null && petStarResponse.Code == 0)
            {
                NetworkUtils.HandleResponse_UpdatePets(petStarResponse.PetDto);
                PetDataModule dataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
                for (int i = 0; i < petStarResponse.PetDto.Count; i++)
                {
                    if (dataModule.IsDeploy(petStarResponse.PetDto[i].RowId))
                    {
                        dataModule.MathAddAttributeData();
                        GameApp.Event.DispatchNow(null, 145, null);
                        break;
                    }
                }
                Action<bool, PetStarResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, petStarResponse);
                return;
            }
            else
            {
                Action<bool, PetStarResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, petStarResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void PetLevelUpRequest(ulong rwoId, bool isQuickLevelUp, Action<bool, PetStrengthResponse> callback)
    {
        PetStrengthRequest petStrengthRequest = new PetStrengthRequest();
        petStrengthRequest.CommonParams = NetworkUtils.GetCommonParams();
        petStrengthRequest.RowId = rwoId;
        if (isQuickLevelUp)
        {
            petStrengthRequest.OptType = 1;
        }
        GameApp.NetWork.Send(petStrengthRequest, delegate (IMessage response)
        {
            PetStrengthResponse petStrengthResponse = response as PetStrengthResponse;
            if (petStrengthResponse != null && petStrengthResponse.Code == 0)
            {
                RepeatedField<PetDto> repeatedField = new RepeatedField<PetDto>();
                repeatedField.Add(petStrengthResponse.PetDto);
                NetworkUtils.HandleResponse_UpdatePets(repeatedField);
                GameApp.Data.GetDataModule(DataName.PetDataModule).MathAddAttributeData();
                GameApp.Event.DispatchNow(null, 145, null);
                Action<bool, PetStrengthResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, petStrengthResponse);
                }
                GameApp.SDK.Analyze.Track_PetLevel(petStrengthResponse.PetDto, petStrengthResponse.CommonData.CostDto);
                return;
            }
            Action<bool, PetStrengthResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, petStrengthResponse);
        }, true, false, string.Empty, true);
    }

    public static void PetResetRequest(ulong rwoId, Action<bool, PetResetResponse> callback)
    {
        PetResetRequest petResetRequest = new PetResetRequest();
        petResetRequest.CommonParams = NetworkUtils.GetCommonParams();
        petResetRequest.RowId = rwoId;
        GameApp.NetWork.Send(petResetRequest, delegate (IMessage response)
        {
            PetResetResponse petResetResponse = response as PetResetResponse;
            if (petResetResponse != null && petResetResponse.Code == 0)
            {
                NetworkUtils.HandleResponse_UpdatePets(petResetResponse.PetDto);
                GameApp.Data.GetDataModule(DataName.PetDataModule).MathAddAttributeData();
                GameApp.Event.DispatchNow(null, 145, null);
                if (petResetResponse.CommonData.Reward.Count > 0)
                {
                    DxxTools.UI.OpenRewardCommon(petResetResponse.CommonData.Reward, null, true);
                }
                Action<bool, PetResetResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, petResetResponse);
                return;
            }
            else
            {
                Action<bool, PetResetResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, petResetResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void PetShowRequest(List<ulong> rowIds, Action<bool, PetShowResponse> callback, bool isShowMask = true)
    {
        PetShowRequest petShowRequest = new PetShowRequest();
        petShowRequest.CommonParams = NetworkUtils.GetCommonParams();
        petShowRequest.RowId.AddRange(rowIds);
        GameApp.NetWork.Send(petShowRequest, delegate (IMessage response)
        {
            PetShowResponse petShowResponse = response as PetShowResponse;
            if (petShowResponse != null && petShowResponse.Code == 0)
            {
                NetworkUtils.HandleResponse_UpdatePets(petShowResponse.PetDtos);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_PetDataModule_ShowIdsChange, null);
                Action<bool, PetShowResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, petShowResponse);
                return;
            }
            else
            {
                Action<bool, PetShowResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, petShowResponse);
                return;
            }
        }, isShowMask, false, string.Empty, true);
    }

    public static void PetFormatPosRequest(List<ulong> rowIds, Action<bool, PetFormationResponse> callback)
    {
        PetFormationRequest petFormationRequest = new PetFormationRequest();
        petFormationRequest.CommonParams = NetworkUtils.GetCommonParams();
        petFormationRequest.RowId.AddRange(rowIds);
        GameApp.NetWork.Send(petFormationRequest, delegate (IMessage response)
        {
            PetFormationResponse petFormationResponse = response as PetFormationResponse;
            if (petFormationResponse != null && petFormationResponse.Code == 0)
            {
                NetworkUtils.HandleResponse_UpdatePets(petFormationResponse.PetDtos);
                PetDataModule dataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
                dataModule.UpdatePetFormationData();
                dataModule.MathAddAttributeData();
                GameApp.Event.DispatchNow(null, 145, null);
                Action<bool, PetFormationResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, petFormationResponse);
                return;
            }
            else
            {
                Action<bool, PetFormationResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, petFormationResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void PetFetterActiveRequest(int configId, Action<bool, PetFetterActiveResponse> callback)
    {
        PetFetterActiveRequest petFetterActiveRequest = new PetFetterActiveRequest();
        petFetterActiveRequest.CommonParams = NetworkUtils.GetCommonParams();
        petFetterActiveRequest.ConfigId = configId;
        GameApp.NetWork.Send(petFetterActiveRequest, delegate (IMessage response)
        {
            PetFetterActiveResponse petFetterActiveResponse = response as PetFetterActiveResponse;
            if (petFetterActiveResponse != null && petFetterActiveResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.PetDataModule).UpdateCollectionData(petFetterActiveResponse.ConfigId);
                GameApp.Data.GetDataModule(DataName.PetDataModule).MathAddAttributeData();
                GameApp.Event.DispatchNow(null, 145, null);
                RedPointController.Instance.ReCalc("Equip.Pet", true);
                Action<bool, PetFetterActiveResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, petFetterActiveResponse);
                return;
            }
            else
            {
                Action<bool, PetFetterActiveResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, petFetterActiveResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void PetTrainRequest(ulong rowId, List<int> lockList, Action<bool, PetTrainResponse> callback)
    {
        PetTrainRequest petTrainRequest = new PetTrainRequest();
        petTrainRequest.CommonParams = NetworkUtils.GetCommonParams();
        petTrainRequest.RowId = rowId;
        if (lockList != null)
        {
            for (int i = 0; i < lockList.Count; i++)
            {
                petTrainRequest.LockIndex.Add(lockList[i]);
            }
        }
        GameApp.NetWork.Send(petTrainRequest, delegate (IMessage response)
        {
            PetTrainResponse petTrainResponse = response as PetTrainResponse;
            if (petTrainResponse != null && petTrainResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.PetDataModule).UpdateTrainingData(petTrainResponse.TrainingLevel, petTrainResponse.TrainingExp);
                RepeatedField<PetDto> repeatedField = new RepeatedField<PetDto>();
                repeatedField.Add(petTrainResponse.PetDto);
                NetworkUtils.HandleResponse_UpdatePets(repeatedField);
                Action<bool, PetTrainResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, petTrainResponse);
                return;
            }
            else
            {
                Action<bool, PetTrainResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, petTrainResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void PetTrainSureRequest(ulong rowId, Action<bool, PetTrainSureResponse> callback)
    {
        PetTrainSureRequest petTrainSureRequest = new PetTrainSureRequest();
        petTrainSureRequest.CommonParams = NetworkUtils.GetCommonParams();
        petTrainSureRequest.RowId = rowId;
        GameApp.NetWork.Send(petTrainSureRequest, delegate (IMessage response)
        {
            PetTrainSureResponse petTrainSureResponse = response as PetTrainSureResponse;
            if (petTrainSureResponse != null && petTrainSureResponse.Code == 0)
            {
                RepeatedField<PetDto> repeatedField = new RepeatedField<PetDto>();
                repeatedField.Add(petTrainSureResponse.PetDto);
                NetworkUtils.HandleResponse_UpdatePets(repeatedField);
                PetDataModule dataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
                if (dataModule.IsDeploy(petTrainSureResponse.PetDto.RowId))
                {
                    dataModule.MathAddAttributeData();
                    GameApp.Event.DispatchNow(null, 145, null);
                }
                Action<bool, PetTrainSureResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, petTrainSureResponse);
                return;
            }
            else
            {
                Action<bool, PetTrainSureResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, petTrainSureResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }
}
