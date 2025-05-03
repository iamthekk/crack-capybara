
public class TalentLegacy
{
    public static void DoTalentLegacyRankRequest(Action<bool, TalentLegacyLeaderBoardResponse> callBack)
    {
        TalentLegacyLeaderBoardRequest talentLegacyLeaderBoardRequest = new TalentLegacyLeaderBoardRequest();
        talentLegacyLeaderBoardRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(talentLegacyLeaderBoardRequest, delegate (IMessage response)
        {
            TalentLegacyLeaderBoardResponse talentLegacyLeaderBoardResponse = response as TalentLegacyLeaderBoardResponse;
            if (talentLegacyLeaderBoardResponse != null && talentLegacyLeaderBoardResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule).OnSetRankInfo(talentLegacyLeaderBoardResponse.UserList);
                Action<bool, TalentLegacyLeaderBoardResponse> callBack2 = callBack;
                if (callBack2 == null)
                {
                    return;
                }
                callBack2(true, talentLegacyLeaderBoardResponse);
                return;
            }
            else
            {
                Action<bool, TalentLegacyLeaderBoardResponse> callBack3 = callBack;
                if (callBack3 == null)
                {
                    return;
                }
                callBack3(false, talentLegacyLeaderBoardResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoTalentLegacyInfoRequest(Action<bool, TalentLegacyInfoResponse> callBack, bool isShowMask = true)
    {
        TalentLegacyInfoRequest talentLegacyInfoRequest = new TalentLegacyInfoRequest();
        talentLegacyInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(talentLegacyInfoRequest, delegate (IMessage response)
        {
            TalentLegacyInfoResponse talentLegacyInfoResponse = response as TalentLegacyInfoResponse;
            if (talentLegacyInfoResponse != null && talentLegacyInfoResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule).OnSetTalentLegacyInfo(talentLegacyInfoResponse);
                RedPointController.Instance.ReCalc("Talent.TalentLegacy", true);
                RedPointController.Instance.ReCalc("Talent.TalentLegacy.TalentLegacyNode", true);
                RedPointController.Instance.ReCalc("Equip.TalentLegacySkill", true);
                Action<bool, TalentLegacyInfoResponse> callBack2 = callBack;
                if (callBack2 == null)
                {
                    return;
                }
                callBack2(true, talentLegacyInfoResponse);
                return;
            }
            else
            {
                Action<bool, TalentLegacyInfoResponse> callBack3 = callBack;
                if (callBack3 == null)
                {
                    return;
                }
                callBack3(false, talentLegacyInfoResponse);
                return;
            }
        }, isShowMask, false, string.Empty, true);
    }

    public static void DoTalentLegacyLevelUpRequest(int careerId, int talentLegacyId, Action<bool, TalentLegacyLevelUpResponse> callBack)
    {
        TalentLegacyLevelUpRequest talentLegacyLevelUpRequest = new TalentLegacyLevelUpRequest();
        talentLegacyLevelUpRequest.CommonParams = NetworkUtils.GetCommonParams();
        talentLegacyLevelUpRequest.CareerId = careerId;
        talentLegacyLevelUpRequest.TalentLegacyId = talentLegacyId;
        GameApp.NetWork.Send(talentLegacyLevelUpRequest, delegate (IMessage response)
        {
            TalentLegacyLevelUpResponse talentLegacyLevelUpResponse = response as TalentLegacyLevelUpResponse;
            if (talentLegacyLevelUpResponse != null && talentLegacyLevelUpResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule).OnTalentLegacyLevelUpBack(talentLegacyLevelUpResponse);
                PlayerPrefsKeys.SetTalentLegacyNode(HLog.StringBuilder(careerId.ToString(), "_", talentLegacyId.ToString()));
                RedPointController.Instance.ReCalc("Talent.TalentLegacy", true);
                RedPointController.Instance.ReCalc("Talent.TalentLegacy.TalentLegacyNode", true);
                GameApp.Event.DispatchNow(null, 465, null);
                GameApp.SDK.Analyze.Track_TalentLegacyStudy(talentLegacyId, talentLegacyLevelUpResponse.CommonData.CostDto);
                Action<bool, TalentLegacyLevelUpResponse> callBack2 = callBack;
                if (callBack2 == null)
                {
                    return;
                }
                callBack2(true, talentLegacyLevelUpResponse);
                return;
            }
            else
            {
                Action<bool, TalentLegacyLevelUpResponse> callBack3 = callBack;
                if (callBack3 == null)
                {
                    return;
                }
                callBack3(false, talentLegacyLevelUpResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoTalentLegacySkillSwitchRequest(int careerId, int talentLegacyId, int slotIndex, Action<bool, TalentLegacySwitchResponse> callBack)
    {
        TalentLegacySwitchRequest talentLegacySwitchRequest = new TalentLegacySwitchRequest();
        talentLegacySwitchRequest.CommonParams = NetworkUtils.GetCommonParams();
        talentLegacySwitchRequest.FromCareerId = careerId;
        talentLegacySwitchRequest.ToTalentLegacyId = talentLegacyId;
        talentLegacySwitchRequest.Index = slotIndex;
        GameApp.NetWork.Send(talentLegacySwitchRequest, delegate (IMessage response)
        {
            TalentLegacySwitchResponse talentLegacySwitchResponse = response as TalentLegacySwitchResponse;
            if (talentLegacySwitchResponse != null && talentLegacySwitchResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule).OnTalentLegacySkillSwitchBack(talentLegacySwitchResponse);
                RedPointController.Instance.ReCalc("Talent.TalentLegacy", true);
                RedPointController.Instance.ReCalc("Equip.TalentLegacySkill", true);
                GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule).MathAddAttributeData();
                GameApp.Event.DispatchNow(null, 145, null);
                GameApp.Event.DispatchNow(null, 469, null);
                Action<bool, TalentLegacySwitchResponse> callBack2 = callBack;
                if (callBack2 == null)
                {
                    return;
                }
                callBack2(true, talentLegacySwitchResponse);
                return;
            }
            else
            {
                Action<bool, TalentLegacySwitchResponse> callBack3 = callBack;
                if (callBack3 == null)
                {
                    return;
                }
                callBack3(false, talentLegacySwitchResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoTalentLegacyLevelUpCoolDownRequest(int careerId, int talentLegacyId, int useType, int useNum, Action<bool, TalentLegacyLevelUpCoolDownResponse> callBack)
    {
        TalentLegacyLevelUpCoolDownRequest talentLegacyLevelUpCoolDownRequest = new TalentLegacyLevelUpCoolDownRequest();
        talentLegacyLevelUpCoolDownRequest.CommonParams = NetworkUtils.GetCommonParams();
        talentLegacyLevelUpCoolDownRequest.CareerId = careerId;
        talentLegacyLevelUpCoolDownRequest.TalentLegacyId = talentLegacyId;
        talentLegacyLevelUpCoolDownRequest.UseType = useType;
        talentLegacyLevelUpCoolDownRequest.UseNum = useNum;
        GameApp.NetWork.Send(talentLegacyLevelUpCoolDownRequest, delegate (IMessage response)
        {
            TalentLegacyLevelUpCoolDownResponse talentLegacyLevelUpCoolDownResponse = response as TalentLegacyLevelUpCoolDownResponse;
            if (talentLegacyLevelUpCoolDownResponse != null && talentLegacyLevelUpCoolDownResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.AdDataModule).UpdateAdData(talentLegacyLevelUpCoolDownResponse.AdData);
                TalentLegacyDataModule dataModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
                bool flag = false;
                if (dataModule.OnGetTalentLegacySkillInfo(careerId, talentLegacyId).LevelUpTime > 0L)
                {
                    flag = true;
                }
                dataModule.OnTalentLegacyLevelUpCoolDownBack(talentLegacyLevelUpCoolDownResponse);
                if (dataModule.OnGetTalentLegacySkillInfo(careerId, talentLegacyId).LevelUpTime <= 0L && flag)
                {
                    GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule).MathAddAttributeData();
                    GameApp.Event.DispatchNow(null, 145, null);
                    dataModule.OpenStudyFinishPanel(careerId, talentLegacyId);
                    EventArgsNodeTimeEnd eventArgsNodeTimeEnd = new EventArgsNodeTimeEnd(careerId, talentLegacyId);
                    GameApp.Event.DispatchNow(null, LocalMessageName.CC_TalentLegacyNodeSpeedEnd, eventArgsNodeTimeEnd);
                }
                RedPointController.Instance.ReCalc("Talent.TalentLegacy", true);
                RedPointController.Instance.ReCalc("Talent.TalentLegacy.TalentLegacyNode", true);
                GameApp.Event.DispatchNow(null, 466, null);
                Action<bool, TalentLegacyLevelUpCoolDownResponse> callBack2 = callBack;
                if (callBack2 == null)
                {
                    return;
                }
                callBack2(true, talentLegacyLevelUpCoolDownResponse);
                return;
            }
            else
            {
                Action<bool, TalentLegacyLevelUpCoolDownResponse> callBack3 = callBack;
                if (callBack3 == null)
                {
                    return;
                }
                callBack3(false, talentLegacyLevelUpCoolDownResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoTalentLegacySelectCareerRequest(int careerId, Action<bool, TalentLegacySelectCareerResponse> callBack)
    {
        TalentLegacySelectCareerRequest talentLegacySelectCareerRequest = new TalentLegacySelectCareerRequest();
        talentLegacySelectCareerRequest.CommonParams = NetworkUtils.GetCommonParams();
        talentLegacySelectCareerRequest.CareerId = careerId;
        GameApp.NetWork.Send(talentLegacySelectCareerRequest, delegate (IMessage response)
        {
            TalentLegacySelectCareerResponse talentLegacySelectCareerResponse = response as TalentLegacySelectCareerResponse;
            if (talentLegacySelectCareerResponse != null && talentLegacySelectCareerResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule).OnTalentLegacySelectCareerBack(talentLegacySelectCareerResponse);
                RedPointController.Instance.ReCalc("Talent.TalentLegacy", true);
                RedPointController.Instance.ReCalc("Equip.TalentLegacySkill", true);
                GameApp.Event.DispatchNow(null, 470, null);
                GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule).MathAddAttributeData();
                GameApp.Event.DispatchNow(null, 145, null);
                Action<bool, TalentLegacySelectCareerResponse> callBack2 = callBack;
                if (callBack2 == null)
                {
                    return;
                }
                callBack2(true, talentLegacySelectCareerResponse);
                return;
            }
            else
            {
                Action<bool, TalentLegacySelectCareerResponse> callBack3 = callBack;
                if (callBack3 == null)
                {
                    return;
                }
                callBack3(false, talentLegacySelectCareerResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }
}
