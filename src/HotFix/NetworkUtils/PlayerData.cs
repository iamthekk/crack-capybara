
public class PlayerData
{
    public static void RequestUnlockUserAvatar(int itemType, int itemId, Action<bool, UnlockUserAvatarResponse> callback, bool isShowMask = true)
    {
        UnlockUserAvatarRequest unlockUserAvatarRequest = new UnlockUserAvatarRequest();
        unlockUserAvatarRequest.CommonParams = NetworkUtils.GetCommonParams();
        unlockUserAvatarRequest.ItemType = (uint)itemType;
        unlockUserAvatarRequest.ItemId = (uint)itemId;
        GameApp.NetWork.Send(unlockUserAvatarRequest, delegate (IMessage response)
        {
            UnlockUserAvatarResponse unlockUserAvatarResponse = response as UnlockUserAvatarResponse;
            if (unlockUserAvatarResponse != null && unlockUserAvatarResponse.Code == 0)
            {
                LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
                dataModule.ServerSetUserInfo(unlockUserAvatarResponse.UserInfoDto, false);
                dataModule.UpdateUnlockAllAvatarClotheScene();
                if (itemType < 3 || itemType == 7)
                {
                    RedPointController.Instance.ReCalc("Main.SelfInfo.Avatar.Avatar", true);
                }
                else if (itemType == 6)
                {
                    RedPointController.Instance.ReCalc("Main.SelfInfo.Avatar.Scene", true);
                }
                else
                {
                    RedPointController.Instance.ReCalc("Main.SelfInfo.Avatar.Clothes", true);
                }
                RedPointController.Instance.ReCalc("Equip.Fashion", true);
                Action<bool, UnlockUserAvatarResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, unlockUserAvatarResponse);
                }
                NetworkUtils.PlayerData.SetUserInfoToChat();
                return;
            }
            Action<bool, UnlockUserAvatarResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, unlockUserAvatarResponse);
        }, isShowMask, false, string.Empty, true);
    }

    public static void RequestUpdateUserAvatar(int partType, int changeTableId, Action<bool, UpdateUserAvatarResponse> callback, bool isShowMask = true)
    {
        LoginDataModule loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
        ClothesData selfClothesData = GameApp.Data.GetDataModule(DataName.ClothesDataModule).SelfClothesData;
        AvatarClothesData avatarClothesData = new AvatarClothesData();
        avatarClothesData.AvatarIconId = loginDataModule.Avatar;
        avatarClothesData.AvatarFrameId = loginDataModule.AvatarFrame;
        avatarClothesData.SceneSkinId = GameApp.Data.GetDataModule(DataName.ClothesDataModule).SelfSceneSkinData.CurSkinId;
        avatarClothesData.ClothesData = new ClothesData(selfClothesData.HeadId, selfClothesData.BodyId, selfClothesData.AccessoryId);
        if (partType == 1)
        {
            avatarClothesData.AvatarIconId = changeTableId;
        }
        else if (partType == 2)
        {
            avatarClothesData.AvatarFrameId = changeTableId;
        }
        else if (partType == 7)
        {
            avatarClothesData.AvatarTitleId = changeTableId;
        }
        else if (partType == 3)
        {
            avatarClothesData.ClothesData.DressPart(SkinType.Body, changeTableId);
        }
        else if (partType == 4)
        {
            avatarClothesData.ClothesData.DressPart(SkinType.Head, changeTableId);
        }
        else if (partType == 5)
        {
            avatarClothesData.ClothesData.DressPart(SkinType.Back, changeTableId);
        }
        else if (partType == 6)
        {
            avatarClothesData.SceneSkinId = changeTableId;
        }
        UpdateUserAvatarRequest updateUserAvatarRequest = new UpdateUserAvatarRequest();
        updateUserAvatarRequest.CommonParams = NetworkUtils.GetCommonParams();
        updateUserAvatarRequest.AvatarId = (uint)avatarClothesData.AvatarIconId;
        updateUserAvatarRequest.AvatarFrameId = (uint)avatarClothesData.AvatarFrameId;
        updateUserAvatarRequest.TitleId = (uint)avatarClothesData.AvatarTitleId;
        updateUserAvatarRequest.SkinHeaddressId = (uint)avatarClothesData.ClothesData.HeadId;
        updateUserAvatarRequest.SkinBodyId = (uint)avatarClothesData.ClothesData.BodyId;
        updateUserAvatarRequest.SkinAccessoryId = (uint)avatarClothesData.ClothesData.AccessoryId;
        updateUserAvatarRequest.BackGround = (uint)avatarClothesData.SceneSkinId;
        GameApp.NetWork.Send(updateUserAvatarRequest, delegate (IMessage response)
        {
            UpdateUserAvatarResponse updateUserAvatarResponse = response as UpdateUserAvatarResponse;
            if (updateUserAvatarResponse != null && updateUserAvatarResponse.Code == 0)
            {
                loginDataModule.ServerSetUserInfo(updateUserAvatarResponse.UserInfoDto, false);
                Action<bool, UpdateUserAvatarResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, updateUserAvatarResponse);
                }
                NetworkUtils.PlayerData.SetUserInfoToChat();
                return;
            }
            Action<bool, UpdateUserAvatarResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, updateUserAvatarResponse);
        }, isShowMask, false, string.Empty, true);
    }

    public static void DoUserUpdateInfoRequest(string nickName, uint avatar, uint avatarframe, Action<bool, UserUpdateInfoResponse> callback, bool isShowMask = false)
    {
        UserUpdateInfoRequest userUpdateInfoRequest = new UserUpdateInfoRequest();
        userUpdateInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
        LoginDataModule ldm = GameApp.Data.GetDataModule(DataName.LoginDataModule);
        if (string.IsNullOrEmpty(nickName))
        {
            userUpdateInfoRequest.NickName = ldm.ServerSetNickName;
        }
        else
        {
            userUpdateInfoRequest.NickName = nickName;
        }
        if (avatar <= 0U)
        {
            userUpdateInfoRequest.Avatar = (uint)ldm.Avatar;
        }
        else
        {
            userUpdateInfoRequest.Avatar = avatar;
        }
        if (avatarframe <= 0U)
        {
            userUpdateInfoRequest.AvatarFrame = (uint)ldm.AvatarFrame;
        }
        else
        {
            userUpdateInfoRequest.AvatarFrame = avatarframe;
        }
        GameApp.NetWork.Send(userUpdateInfoRequest, delegate (IMessage response)
        {
            UserUpdateInfoResponse userUpdateInfoResponse = response as UserUpdateInfoResponse;
            if (userUpdateInfoResponse != null && userUpdateInfoResponse.Code == 0)
            {
                ldm.ServerSetUserInfo(userUpdateInfoResponse.UserInfoDto, false);
                Action<bool, UserUpdateInfoResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, userUpdateInfoResponse);
                }
                NetworkUtils.PlayerData.SetUserInfoToChat();
                return;
            }
            HLog.LogError(string.Format("玩家信息-同步昵称失败 nickName = {0},Code = {1}", nickName, userUpdateInfoResponse.Code));
            Action<bool, UserUpdateInfoResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, userUpdateInfoResponse);
        }, isShowMask, false, string.Empty, true);
    }

    public static void UserGetPlayerInfoRequest(Action<bool, UserGetPlayerInfoResponse> callback, long userid)
    {
        UserGetPlayerInfoRequest userGetPlayerInfoRequest = new UserGetPlayerInfoRequest();
        userGetPlayerInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
        userGetPlayerInfoRequest.PlayerUserId = userid;
        GameApp.NetWork.Send(userGetPlayerInfoRequest, delegate (IMessage response)
        {
            UserGetPlayerInfoResponse userGetPlayerInfoResponse = response as UserGetPlayerInfoResponse;
            if (userGetPlayerInfoResponse != null && userGetPlayerInfoResponse.Code == 0)
            {
                Action<bool, UserGetPlayerInfoResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, userGetPlayerInfoResponse);
                return;
            }
            else
            {
                Action<bool, UserGetPlayerInfoResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, userGetPlayerInfoResponse);
                return;
            }
        }, false, false, string.Empty, false);
    }

    private static void SetUserInfoToChat()
    {
    }

    public static void TipSendUserGetInfoRequest(string contextKey, Action success = null)
    {
        if (NetworkUtils.PlayerData.isTipAndSendUserGetInfoRequesting)
        {
            return;
        }
        NetworkUtils.PlayerData.isTipAndSendUserGetInfoRequesting = true;
        string text = string.Format(Singleton<LanguageManager>.Instance.GetInfoByID(contextKey), Array.Empty<object>());
        string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("17");
        Action<bool, UserGetIapInfoResponse> <> 9__1;
        DxxTools.UI.OpenPopCommon(text, delegate (int id)
        {
            Action<bool, UserGetIapInfoResponse> action;
            if ((action = <> 9__1) == null)
            {
                action = (<> 9__1 = delegate (bool isOk, UserGetIapInfoResponse response)
                {
                    NetworkUtils.PlayerData.isTipAndSendUserGetInfoRequesting = false;
                    Action success2 = success;
                    if (success2 == null)
                    {
                        return;
                    }
                    success2();
                });
            }
            NetworkUtils.PlayerData.SendUserGetIapInfoRequest(action);
        }, string.Empty, infoByID, string.Empty, false, 2);
    }

    public static void SendUserGetIapInfoRequest(Action success)
    {
        if (NetworkUtils.PlayerData.isTipAndSendUserGetInfoRequesting)
        {
            return;
        }
        NetworkUtils.PlayerData.isTipAndSendUserGetInfoRequesting = true;
        NetworkUtils.PlayerData.SendUserGetIapInfoRequest(delegate (bool isOk, UserGetIapInfoResponse response)
        {
            NetworkUtils.PlayerData.isTipAndSendUserGetInfoRequesting = false;
            Action success2 = success;
            if (success2 == null)
            {
                return;
            }
            success2();
        });
    }

    private static void SendUserGetIapInfoRequest(Action<bool, UserGetIapInfoResponse> callback)
    {
        UserGetIapInfoRequest userGetIapInfoRequest = new UserGetIapInfoRequest();
        userGetIapInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(userGetIapInfoRequest, delegate (IMessage response)
        {
            UserGetIapInfoResponse userGetIapInfoResponse = response as UserGetIapInfoResponse;
            if (userGetIapInfoResponse != null && userGetIapInfoResponse.Code == 0)
            {
                EventArgsRefreshIAPInfoData eventArgsRefreshIAPInfoData = new EventArgsRefreshIAPInfoData();
                eventArgsRefreshIAPInfoData.SetData(userGetIapInfoResponse.IapInfo);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshIAPInfoData, eventArgsRefreshIAPInfoData);
                Action<bool, UserGetIapInfoResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, userGetIapInfoResponse);
                return;
            }
            else
            {
                Action<bool, UserGetIapInfoResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, userGetIapInfoResponse);
                return;
            }
        }, false, false, string.Empty, false);
    }

    public static void UserOpenModelRequest(List<uint> functionIds, Action<bool, UserOpenModelResponse> callback)
    {
        UserOpenModelRequest userOpenModelRequest = new UserOpenModelRequest();
        userOpenModelRequest.CommonParams = NetworkUtils.GetCommonParams();
        userOpenModelRequest.ModelIds.AddRange(functionIds);
        GameApp.NetWork.Send(userOpenModelRequest, delegate (IMessage response)
        {
            UserOpenModelResponse userOpenModelResponse = response as UserOpenModelResponse;
            if (userOpenModelResponse != null && userOpenModelResponse.Code == 0)
            {
                Action<bool, UserOpenModelResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, userOpenModelResponse);
                return;
            }
            else
            {
                Action<bool, UserOpenModelResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, userOpenModelResponse);
                return;
            }
        }, false, false, string.Empty, false);
    }

    public static void SendUserGetAllPanelInfoRequest(Action<bool, UserGetAllPanelInfoResponse> callback)
    {
        UserGetAllPanelInfoRequest userGetAllPanelInfoRequest = new UserGetAllPanelInfoRequest();
        userGetAllPanelInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(userGetAllPanelInfoRequest, delegate (IMessage response)
        {
            UserGetAllPanelInfoResponse userGetAllPanelInfoResponse = response as UserGetAllPanelInfoResponse;
            if (userGetAllPanelInfoResponse != null && userGetAllPanelInfoResponse.Code == 0)
            {
                Action<bool, UserGetAllPanelInfoResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, userGetAllPanelInfoResponse);
                return;
            }
            else
            {
                Action<bool, UserGetAllPanelInfoResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, userGetAllPanelInfoResponse);
                return;
            }
        }, false, false, string.Empty, false);
    }

    private static bool isTipAndSendUserGetInfoRequesting;
}
