
public class Tower
{
    public static void TowerChallengeRequest(int levelConfigId, Action<bool, TowerChallengeResponse> callback)
    {
        TowerDataModule dataModule = GameApp.Data.GetDataModule(DataName.TowerDataModule);
        int num = dataModule.CalculateShouldChallengeLevelID(dataModule.CompleteTowerLevelId);
        TowerChallenge_Tower towerConfigByLevelId = dataModule.GetTowerConfigByLevelId(num);
        int towerNum = dataModule.GetTowerConfigNum(towerConfigByLevelId);
        int levelNum = dataModule.GetLevelNumByLevelId(num);
        TicketDataModule ticketData = GameApp.Data.GetDataModule(DataName.TicketDataModule);
        GameApp.SDK.Analyze.Track_TowerBattleStart(towerNum, levelNum, (int)ticketData.GetTicket(UserTicketKind.Tower).NewNum);
        TowerChallengeRequest towerChallengeRequest = new TowerChallengeRequest
        {
            CommonParams = NetworkUtils.GetCommonParams(),
            ConfigId = (uint)levelConfigId,
            ClientVersion = Singleton<BattleServerVersionMgr>.Instance.GetVersion()
        };
        GameApp.NetWork.Send(towerChallengeRequest, delegate (IMessage response)
        {
            TowerChallengeResponse towerChallengeResponse = response as TowerChallengeResponse;
            if (towerChallengeResponse != null && towerChallengeResponse.Code == 0)
            {
                RedPointController.Instance.ReCalc("Main.NewWorld", true);
                GameApp.SDK.Analyze.Track_TowerBattleEnd(towerNum, levelNum, (int)ticketData.GetTicket(UserTicketKind.Tower).NewNum, (int)towerChallengeResponse.Result, towerChallengeResponse.CommonData.Reward);
            }
            Action<bool, TowerChallengeResponse> callback2 = callback;
            if (callback2 == null)
            {
                return;
            }
            callback2(towerChallengeResponse != null && towerChallengeResponse.Code == 0, towerChallengeResponse);
        }, true, false, string.Empty, true);
    }

    public static void TowerRewardRequest(int towerConfigId, Action<bool, TowerRewardResponse> callback)
    {
        TowerRewardRequest towerRewardRequest = new TowerRewardRequest
        {
            CommonParams = NetworkUtils.GetCommonParams(),
            ConfigId = (uint)towerConfigId
        };
        GameApp.NetWork.Send(towerRewardRequest, delegate (IMessage response)
        {
            TowerRewardResponse towerRewardResponse = response as TowerRewardResponse;
            bool flag = towerRewardResponse != null && towerRewardResponse.Code == 0;
            Action<bool, TowerRewardResponse> callback2 = callback;
            if (callback2 == null)
            {
                return;
            }
            callback2(flag, towerRewardResponse);
        }, true, false, string.Empty, true);
    }

    public static void TowerRankRequest(int page, bool isShowMask, Action<bool, TowerRankResponse> callback)
    {
        TowerRankRequest towerRankRequest = new TowerRankRequest
        {
            CommonParams = NetworkUtils.GetCommonParams(),
            Page = page
        };
        GameApp.NetWork.Send(towerRankRequest, delegate (IMessage response)
        {
            TowerRankResponse towerRankResponse = response as TowerRankResponse;
            Action<bool, TowerRankResponse> callback2 = callback;
            if (callback2 == null)
            {
                return;
            }
            callback2(towerRankResponse != null && towerRankResponse.Code == 0, towerRankResponse);
        }, isShowMask, false, string.Empty, true);
    }

    public static void TowerRankIndexRequest(bool isShowMask, Action<bool, TowerRankIndexResponse> callback)
    {
        TowerRankIndexRequest towerRankIndexRequest = new TowerRankIndexRequest
        {
            CommonParams = NetworkUtils.GetCommonParams()
        };
        GameApp.NetWork.Send(towerRankIndexRequest, delegate (IMessage response)
        {
            TowerRankIndexResponse towerRankIndexResponse = response as TowerRankIndexResponse;
            bool flag = towerRankIndexResponse != null && towerRankIndexResponse.Code == 0;
            if (flag)
            {
                EventArgsSetCurTowerRankData instance = Singleton<EventArgsSetCurTowerRankData>.Instance;
                instance.TowerRank = (int)towerRankIndexResponse.Index;
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_TowerDataMoudule_SetCurTowerRankData, instance);
            }
            Action<bool, TowerRankIndexResponse> callback2 = callback;
            if (callback2 == null)
            {
                return;
            }
            callback2(flag, towerRankIndexResponse);
        }, isShowMask, false, string.Empty, true);
    }
}
