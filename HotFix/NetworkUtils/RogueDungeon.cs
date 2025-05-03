
public class RogueDungeon
{
    public static void DoGetPanelInfoRequest(Action<bool, HellGetPanelInfoResponse> callback)
    {
        HellGetPanelInfoRequest hellGetPanelInfoRequest = new HellGetPanelInfoRequest();
        hellGetPanelInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(hellGetPanelInfoRequest, delegate (IMessage response)
        {
            HellGetPanelInfoResponse hellGetPanelInfoResponse = response as HellGetPanelInfoResponse;
            if (hellGetPanelInfoResponse == null || hellGetPanelInfoResponse.Code != 0)
            {
                Action<bool, HellGetPanelInfoResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(false, hellGetPanelInfoResponse);
                }
                return;
            }
            GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule).UpdateBaseInfo(hellGetPanelInfoResponse);
            Action<bool, HellGetPanelInfoResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(true, hellGetPanelInfoResponse);
        }, true, false, string.Empty, true);
    }

    public static void DoEnterChallengeRequest(Action<bool, HellEnterBattleResponse> callback)
    {
        HellEnterBattleRequest hellEnterBattleRequest = new HellEnterBattleRequest();
        hellEnterBattleRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(hellEnterBattleRequest, delegate (IMessage response)
        {
            HellEnterBattleResponse hellEnterBattleResponse = response as HellEnterBattleResponse;
            if (hellEnterBattleResponse == null || hellEnterBattleResponse.Code != 0)
            {
                Action<bool, HellEnterBattleResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(false, hellEnterBattleResponse);
                }
                return;
            }
            GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule).EnterChallenge(hellEnterBattleResponse);
            Action<bool, HellEnterBattleResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(true, hellEnterBattleResponse);
        }, true, false, string.Empty, true);
    }

    public static void DoHellEnterSelectSkillRequest(List<int> skills, Action<bool, HellEnterSelectSkillResponse> callback)
    {
        HellEnterSelectSkillRequest hellEnterSelectSkillRequest = new HellEnterSelectSkillRequest();
        hellEnterSelectSkillRequest.CommonParams = NetworkUtils.GetCommonParams();
        for (int i = 0; i < skills.Count; i++)
        {
            hellEnterSelectSkillRequest.SkillList.Add(skills[i]);
        }
        GameApp.NetWork.Send(hellEnterSelectSkillRequest, delegate (IMessage response)
        {
            HellEnterSelectSkillResponse hellEnterSelectSkillResponse = response as HellEnterSelectSkillResponse;
            if (hellEnterSelectSkillResponse == null || hellEnterSelectSkillResponse.Code != 0)
            {
                Action<bool, HellEnterSelectSkillResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(false, hellEnterSelectSkillResponse);
                }
                return;
            }
            GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule).SetRoundEnterSkillSelected();
            Action<bool, HellEnterSelectSkillResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(true, hellEnterSelectSkillResponse);
        }, true, false, string.Empty, true);
    }

    public static void DoBattleRequest(uint floorId, Action<bool, HellDoChallengeResponse> callback)
    {
        HellDoChallengeRequest hellDoChallengeRequest = new HellDoChallengeRequest();
        hellDoChallengeRequest.CommonParams = NetworkUtils.GetCommonParams();
        hellDoChallengeRequest.ClientVersion = Singleton<BattleServerVersionMgr>.Instance.GetVersion();
        hellDoChallengeRequest.StageId = (int)floorId;
        GameApp.NetWork.Send(hellDoChallengeRequest, delegate (IMessage response)
        {
            HellDoChallengeResponse hellDoChallengeResponse = response as HellDoChallengeResponse;
            if (hellDoChallengeResponse == null || hellDoChallengeResponse.Code != 0)
            {
                Action<bool, HellDoChallengeResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(false, hellDoChallengeResponse);
                }
                return;
            }
            if (hellDoChallengeResponse.Result >= 0)
            {
                GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule).UpdateBattleInfo(hellDoChallengeResponse);
            }
            Action<bool, HellDoChallengeResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(true, hellDoChallengeResponse);
        }, false, false, string.Empty, true);
    }

    public static void DoHellSaveSkillRequest(long currentHp, List<int> skills, Dictionary<string, int> attDic, Action<bool, HellSaveSkillResponse> callback)
    {
        HellSaveSkillRequest hellSaveSkillRequest = new HellSaveSkillRequest();
        hellSaveSkillRequest.CommonParams = NetworkUtils.GetCommonParams();
        hellSaveSkillRequest.Hp = currentHp;
        foreach (int num in skills)
        {
            hellSaveSkillRequest.SkillList.Add(num);
        }
        foreach (string text in attDic.Keys)
        {
            hellSaveSkillRequest.AttrMap.Add(text, attDic[text]);
        }
        GameApp.NetWork.Send(hellSaveSkillRequest, delegate (IMessage response)
        {
            HellSaveSkillResponse hellSaveSkillResponse = response as HellSaveSkillResponse;
            if (hellSaveSkillResponse == null || hellSaveSkillResponse.Code != 0)
            {
                Action<bool, HellSaveSkillResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(false, hellSaveSkillResponse);
                }
                return;
            }
            RogueDungeonDataModule dataModule = GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule);
            dataModule.SetRoundEnter();
            dataModule.UpdateAttribute(hellSaveSkillResponse.AttrMap);
            Action<bool, HellSaveSkillResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(true, hellSaveSkillResponse);
        }, true, false, string.Empty, true);
    }

    public static void DoHellExitBattleRequest(Action<bool, HellExitBattleResponse> callback)
    {
        HellExitBattleRequest hellExitBattleRequest = new HellExitBattleRequest();
        hellExitBattleRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(hellExitBattleRequest, delegate (IMessage response)
        {
            HellExitBattleResponse hellExitBattleResponse = response as HellExitBattleResponse;
            if (hellExitBattleResponse == null || hellExitBattleResponse.Code != 0)
            {
                Action<bool, HellExitBattleResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(false, hellExitBattleResponse);
                }
                return;
            }
            GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule).Escape(hellExitBattleResponse.CommonData.Reward);
            Action<bool, HellExitBattleResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(true, hellExitBattleResponse);
        }, true, false, string.Empty, true);
    }

    public static void DoHellRevertHpRequest(Action<bool, HellRevertHpResponse> callback)
    {
        HellRevertHpRequest hellRevertHpRequest = new HellRevertHpRequest();
        hellRevertHpRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(hellRevertHpRequest, delegate (IMessage response)
        {
            HellRevertHpResponse hellRevertHpResponse = response as HellRevertHpResponse;
            if (hellRevertHpResponse == null || hellRevertHpResponse.Code != 0)
            {
                Action<bool, HellRevertHpResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(false, hellRevertHpResponse);
                }
                return;
            }
            Action<bool, HellRevertHpResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(true, hellRevertHpResponse);
        }, false, false, string.Empty, true);
    }

    public static void DoHellRankRequest(int page, bool isNextPage, bool isShowMask, Action<int, bool, bool, HellRankResponse> callback)
    {
        HellRankRequest hellRankRequest = new HellRankRequest();
        hellRankRequest.CommonParams = NetworkUtils.GetCommonParams();
        hellRankRequest.Page = page;
        GameApp.NetWork.Send(hellRankRequest, delegate (IMessage response)
        {
            HellRankResponse hellRankResponse = response as HellRankResponse;
            if (hellRankResponse == null || hellRankResponse.Code != 0)
            {
                Action<int, bool, bool, HellRankResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(page, isNextPage, false, hellRankResponse);
                }
                return;
            }
            GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule).UpdateRank(page, hellRankResponse);
            Action<int, bool, bool, HellRankResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(page, isNextPage, true, hellRankResponse);
        }, isShowMask, false, string.Empty, true);
    }
}
