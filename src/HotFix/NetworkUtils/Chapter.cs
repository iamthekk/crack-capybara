
public class Chapter
{
    public static void DoStartChapterRequest(int chapterID, Action<bool, StartChapterResponse> callback)
    {
        StartChapterRequest startChapterRequest = new StartChapterRequest();
        startChapterRequest.CommonParams = NetworkUtils.GetCommonParams();
        startChapterRequest.ChapterId = chapterID;
        GameApp.NetWork.Send(startChapterRequest, delegate (IMessage response)
        {
            StartChapterResponse startChapterResponse = response as StartChapterResponse;
            if (startChapterResponse != null && startChapterResponse.Code == 0)
            {
                EventArgServerData eventArgServerData = new EventArgServerData();
                eventArgServerData.SetData(startChapterResponse.ChapterSeed, startChapterResponse.EventMap, startChapterResponse.ActiveMap, startChapterResponse.BattleKey, startChapterResponse.BattleTimes);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_GameEventData_SetServerData, eventArgServerData);
                Action<bool, StartChapterResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, startChapterResponse);
                }
                GameAFTools.Ins.OnStartChapter(startChapterResponse.ChapterId);
                GameApp.SDK.Analyze.Track_ChapterStart(startChapterResponse.ChapterId, startChapterResponse.BattleKey, 0, 0, (int)startChapterResponse.BattleTimes);
                return;
            }
            Action<bool, StartChapterResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, startChapterResponse);
        }, true, false, string.Empty, true);
    }

    public static void DoEndChapterRequest(int chapterID, int stage, int result, string pveData, string battleKey, List<RewardDto> eventReward, List<RewardDto> battleReward, List<int> skills, Action<bool, EndChapterResponse> callback)
    {
        EndChapterRequest endChapterRequest = new EndChapterRequest();
        endChapterRequest.CommonParams = NetworkUtils.GetCommonParams();
        endChapterRequest.ChapterId = chapterID;
        endChapterRequest.WaveIndex = stage;
        endChapterRequest.Result = result;
        endChapterRequest.FightData = pveData;
        for (int i = 0; i < skills.Count; i++)
        {
            endChapterRequest.SkillIds.Add(skills[i]);
        }
        ChapterDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
        int battleTime = dataModule.ChapterBattleTimes;
        GameApp.NetWork.Send(endChapterRequest, delegate (IMessage response)
        {
            Singleton<EventRecordController>.Instance.DeleteChapterRecord();
            EndChapterResponse endChapterResponse = response as EndChapterResponse;
            if (endChapterResponse != null && endChapterResponse.Code == 0)
            {
                Action<bool, EndChapterResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, endChapterResponse);
                }
                RedPointController.Instance.ReCalc("Main.ChapterReward", true);
                RedPointController.Instance.ReCalc("Main.NewWorld", true);
                if (endChapterResponse.CommonData.UpdateUserCurrency != null && endChapterResponse.CommonData.UpdateUserCurrency.UserCurrency != null)
                {
                    long coins = endChapterResponse.CommonData.UpdateUserCurrency.UserCurrency.Coins;
                }
                bool flag = endChapterResponse.ChapterId > chapterID;
                GameAFTools.Ins.OnEndChapter(flag, chapterID);
                BattleChapterPlayerData playerData = Singleton<GameEventController>.Instance.PlayerData;
                int num = 0;
                if (GameTGATools.Ins.ChapterEndQuitType == 0)
                {
                    num = (flag ? 0 : 1);
                }
                else if (GameTGATools.Ins.ChapterEndQuitType == 1)
                {
                    num = 2;
                }
                else if (GameTGATools.Ins.ChapterEndQuitType == 2)
                {
                    num = 3;
                }
                int battleTime2 = battleTime;
                if (playerData != null)
                {
                    GameApp.SDK.Analyze.Track_ChapterEnd(chapterID, battleKey, 0, playerData.Attack.GetValue(), playerData.HpMax.GetValue(), playerData.Defence.GetValue(), battleTime2, playerData.GetPlayerSkillBuildList(), num, Singleton<GameEventController>.Instance.GetCurrentStage(), 0, 0, endChapterResponse.CommonData.Reward, playerData.Chips.mVariable, new List<NodeScoreParam>());
                    return;
                }
                List<GameEventSkillBuildData> list = new List<GameEventSkillBuildData>();
                if (skills != null)
                {
                    foreach (int num2 in skills)
                    {
                        GameSkillBuild_skillBuild elementById = GameApp.Table.GetManager().GetGameSkillBuild_skillBuildModelInstance().GetElementById(num2);
                        if (elementById != null)
                        {
                            GameEventSkillBuildData gameEventSkillBuildData = new GameEventSkillBuildData();
                            gameEventSkillBuildData.SetTable(elementById);
                            list.Add(gameEventSkillBuildData);
                        }
                    }
                }
                GameApp.SDK.Analyze.Track_ChapterEnd(chapterID, battleKey, 0, 0L, 0L, 0L, battleTime2, list, num, Singleton<GameEventController>.Instance.GetCurrentStage(), 0, 0, endChapterResponse.CommonData.Reward, 0, new List<NodeScoreParam>());
                return;
            }
            else
            {
                Action<bool, EndChapterResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, endChapterResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoGetWaveRewardRequest(int chapterID, int stage, Action<bool, GetWaveRewardResponse> callback)
    {
        GetWaveRewardRequest getWaveRewardRequest = new GetWaveRewardRequest();
        getWaveRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
        getWaveRewardRequest.ChapterId = chapterID;
        getWaveRewardRequest.WaveIndex = stage;
        GameApp.NetWork.Send(getWaveRewardRequest, delegate (IMessage response)
        {
            GetWaveRewardResponse getWaveRewardResponse = response as GetWaveRewardResponse;
            if (getWaveRewardResponse != null && getWaveRewardResponse.Code == 0)
            {
                EventArgsRefreshChapterRewardData eventArgsRefreshChapterRewardData = new EventArgsRefreshChapterRewardData();
                eventArgsRefreshChapterRewardData.SetData(getWaveRewardResponse.CanRewardList);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_ChapterData_RefreshChapterRewardData, eventArgsRefreshChapterRewardData);
                Action<bool, GetWaveRewardResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, getWaveRewardResponse);
                }
                RedPointController.Instance.ReCalc("Main.ChapterReward", true);
                GameApp.SDK.Analyze.Track_ChapterReward(chapterID, stage, getWaveRewardResponse.CommonData.Reward);
                return;
            }
            Action<bool, GetWaveRewardResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, getWaveRewardResponse);
        }, true, false, string.Empty, true);
    }

    public static void DoChapterActRewardRequest(int stage, List<ulong> rowIds, Action<bool, ChapterActRewardResponse> callback)
    {
        GameApp.SDK.Analyze.Track_StagetClickTest(null);
        ChapterActRewardRequest chapterActRewardRequest = new ChapterActRewardRequest();
        chapterActRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
        chapterActRewardRequest.Day = stage;
        for (int i = 0; i < rowIds.Count; i++)
        {
            chapterActRewardRequest.RowIds.Add(rowIds[i]);
        }
        ChapterActivityDataModule chapterActivityDataModule = GameApp.Data.GetDataModule(DataName.ChapterActivityDataModule);
        Dictionary<ulong, int> preScores = new Dictionary<ulong, int>();
        if (rowIds.Count > 0)
        {
            foreach (ulong num in rowIds)
            {
                ChapterActivityData activityData = chapterActivityDataModule.GetActivityData(num);
                preScores[num] = (int)activityData.TotalScore;
            }
        }
        GameApp.NetWork.Send(chapterActRewardRequest, delegate (IMessage response)
        {
            ChapterActRewardResponse chapterActRewardResponse = response as ChapterActRewardResponse;
            if (chapterActRewardResponse != null && chapterActRewardResponse.Code == 0)
            {
                Singleton<EventRecordController>.Instance.EventGroupEnd();
                EventArgsChapterActivityRefreshScore eventArgsChapterActivityRefreshScore = new EventArgsChapterActivityRefreshScore();
                eventArgsChapterActivityRefreshScore.SetData(chapterActRewardResponse.Score, chapterActRewardResponse.CommonData.Reward.ToItemDatas());
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_ChapterActivity_RefreshScore, eventArgsChapterActivityRefreshScore);
                Action<bool, ChapterActRewardResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, chapterActRewardResponse);
                }
                if (rowIds.Count <= 0)
                {
                    return;
                }
                ChapterActivityData activityData2 = chapterActivityDataModule.GetActivityData(rowIds[0]);
                if (activityData2 != null)
                {
                    string text = GameTGATools.GetSourceName(11010) + Singleton<LanguageManager>.Instance.GetInfoByID(2, activityData2.ActivityTitleId);
                    GameApp.SDK.Analyze.Track_Get(text, chapterActRewardResponse.CommonData.Reward, null, null, null, null);
                }
                Dictionary<ulong, int> dictionary = new Dictionary<ulong, int>();
                foreach (ulong num2 in rowIds)
                {
                    ChapterActivityData activityData3 = chapterActivityDataModule.GetActivityData(num2);
                    dictionary[num2] = (int)activityData3.TotalScore;
                }
                using (Dictionary<ulong, int>.Enumerator enumerator3 = dictionary.GetEnumerator())
                {
                    while (enumerator3.MoveNext())
                    {
                        KeyValuePair<ulong, int> keyValuePair = enumerator3.Current;
                        int num3;
                        if (preScores.ContainsKey(keyValuePair.Key))
                        {
                            num3 = keyValuePair.Value - preScores[keyValuePair.Key];
                        }
                        else
                        {
                            num3 = keyValuePair.Value;
                        }
                        if (num3 > 0)
                        {
                            string activityTitleId = chapterActivityDataModule.GetActivityData(keyValuePair.Key).ActivityTitleId;
                            string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(2, activityTitleId);
                            int value = keyValuePair.Value;
                            GameApp.SDK.Analyze.Track_ActivityPoint(infoByID, num3, value);
                        }
                    }
                    return;
                }
            }
            Action<bool, ChapterActRewardResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, chapterActRewardResponse);
        }, true, false, string.Empty, true);
    }

    public static void DoStartChapterSweepRequest(int rate, Action<bool, StartChapterSweepResponse> callback)
    {
        StartChapterSweepRequest startChapterSweepRequest = new StartChapterSweepRequest();
        startChapterSweepRequest.CommonParams = NetworkUtils.GetCommonParams();
        startChapterSweepRequest.Rate = (uint)rate;
        GameApp.NetWork.Send(startChapterSweepRequest, delegate (IMessage response)
        {
            StartChapterSweepResponse startChapterSweepResponse = response as StartChapterSweepResponse;
            if (startChapterSweepResponse != null && startChapterSweepResponse.Code == 0)
            {
                Action<bool, StartChapterSweepResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, startChapterSweepResponse);
                }
                GameApp.SDK.Analyze.Track_ChapterStart(startChapterSweepResponse.ChapterId, "", 1, (int)startChapterSweepResponse.Rate, 0);
                return;
            }
            Action<bool, StartChapterSweepResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, startChapterSweepResponse);
        }, true, false, string.Empty, true);
    }

    public static void DoEndChapterSweepRequest(int chapterID, int rate, int stage, List<RewardDto> eventReward, List<RewardDto> battleReward, Action<bool, EndChapterSweepResponse> callback)
    {
        EndChapterSweepRequest endChapterSweepRequest = new EndChapterSweepRequest();
        endChapterSweepRequest.CommonParams = NetworkUtils.GetCommonParams();
        endChapterSweepRequest.ChapterId = chapterID;
        endChapterSweepRequest.WaveIndex = stage;
        GameApp.NetWork.Send(endChapterSweepRequest, delegate (IMessage response)
        {
            Singleton<EventRecordController>.Instance.DeleteSweepRecord();
            EndChapterSweepResponse endChapterSweepResponse = response as EndChapterSweepResponse;
            if (endChapterSweepResponse != null && endChapterSweepResponse.Code == 0)
            {
                Action<bool, EndChapterSweepResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, endChapterSweepResponse);
                }
                if (endChapterSweepResponse.CommonData.UpdateUserCurrency != null && endChapterSweepResponse.CommonData.UpdateUserCurrency.UserCurrency != null)
                {
                    long coins = endChapterSweepResponse.CommonData.UpdateUserCurrency.UserCurrency.Coins;
                }
                GameApp.SDK.Analyze.Track_ChapterEnd(chapterID, "", 1, 0L, 0L, 0L, 0, null, 0, GameApp.Table.GetManager().GetChapter_chapterModelInstance().GetElementById(chapterID)
                    .journeyStage, rate, 0, endChapterSweepResponse.CommonData.Reward, 0, new List<NodeScoreParam>());
                NetworkUtils.Chapter.DoGetHangUpInfoRequest(null);
                return;
            }
            Action<bool, EndChapterSweepResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, endChapterSweepResponse);
        }, true, false, string.Empty, true);
    }

    public static void DoChapterActRankRequest(uint actId, Action<bool, ChapterActRankResponse> callback)
    {
        ChapterActRankRequest chapterActRankRequest = new ChapterActRankRequest();
        chapterActRankRequest.CommonParams = NetworkUtils.GetCommonParams();
        chapterActRankRequest.ActId = (int)actId;
        GameApp.NetWork.Send(chapterActRankRequest, delegate (IMessage response)
        {
            ChapterActRankResponse chapterActRankResponse = response as ChapterActRankResponse;
            if (chapterActRankResponse != null && chapterActRankResponse.Code == 0)
            {
                EventArgsChapterActivityRankReward eventArgsChapterActivityRankReward = new EventArgsChapterActivityRankReward();
                eventArgsChapterActivityRankReward.SetData(chapterActRankResponse.RewardInfo);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_ChapterActivity_RankReward, eventArgsChapterActivityRankReward);
                Action<bool, ChapterActRankResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, chapterActRankResponse);
                return;
            }
            else
            {
                Action<bool, ChapterActRankResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, chapterActRankResponse);
                return;
            }
        }, false, false, string.Empty, true);
    }

    public static void DoChapterRankRewardRequest(Action<bool, ChapterRankRewardResponse> callback)
    {
        ChapterRankRewardRequest chapterRankRewardRequest = new ChapterRankRewardRequest();
        chapterRankRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(chapterRankRewardRequest, delegate (IMessage response)
        {
            ChapterRankRewardResponse chapterRankRewardResponse = response as ChapterRankRewardResponse;
            if (chapterRankRewardResponse != null && chapterRankRewardResponse.Code == 0)
            {
                Action<bool, ChapterRankRewardResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, chapterRankRewardResponse);
                return;
            }
            else
            {
                Action<bool, ChapterRankRewardResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, chapterRankRewardResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoEndChapterCheckRequest(int chapterId, int stage, string attribute, List<int> skills, List<int> monsterCfgIds, long hp, int reviveCount, Action<bool, EndChapterCheckResponse> callback)
    {
        EndChapterCheckRequest request = new EndChapterCheckRequest();
        request.CommonParams = NetworkUtils.GetCommonParams();
        request.ChapterId = chapterId;
        request.WaveIndex = stage;
        request.Attributes = attribute;
        for (int i = 0; i < skills.Count; i++)
        {
            request.SkillIds.Add(skills[i]);
        }
        for (int j = 0; j < monsterCfgIds.Count; j++)
        {
            request.MonsterCfgId.Add(monsterCfgIds[j]);
        }
        request.CurHp = (ulong)hp;
        request.ReviveCount = reviveCount;
        request.ClientVersion = Singleton<BattleServerVersionMgr>.Instance.GetVersion();
        GameApp.NetWork.Send(request, delegate (IMessage response)
        {
            EndChapterCheckResponse endChapterCheckResponse = response as EndChapterCheckResponse;
            if (endChapterCheckResponse != null && endChapterCheckResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.ChapterDataModule).SetBossBattleCheckInfo(request, endChapterCheckResponse);
                Action<bool, EndChapterCheckResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, endChapterCheckResponse);
                return;
            }
            else
            {
                if (endChapterCheckResponse != null && (endChapterCheckResponse.Code == 6004 || endChapterCheckResponse.Code == 6005))
                {
                    string text = string.Format(Singleton<LanguageManager>.Instance.GetInfoByID("battle_check_fail"), Array.Empty<object>());
                    string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("battle_check_ok");
                    DxxTools.UI.OpenPopCommon(text, delegate (int id)
                    {
                        GameApp.View.CloseView(ViewName.GameEventViewModule, null);
                        GameApp.View.CloseAllView(new int[] { 214, 101, 102, 106 });
                        GameApp.State.ActiveState(StateName.LoginState);
                    }, string.Empty, infoByID, string.Empty, false, 2);
                    BattleChapterPlayerData playerData = Singleton<GameEventController>.Instance.PlayerData;
                    GameApp.SDK.Analyze.Track_ChapterBattleCheat(chapterId, stage, playerData.Attack.GetValue(), playerData.Defence.GetValue(), playerData.CurrentHp.GetValue(), playerData.HpMax.GetValue(), playerData.GetPlayerSkillBuildList());
                }
                Action<bool, EndChapterCheckResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, endChapterCheckResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoGetHangUpInfoRequest(Action<bool, GetHangUpInfoResponse> callback)
    {
        GetHangUpInfoRequest getHangUpInfoRequest = new GetHangUpInfoRequest();
        getHangUpInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(getHangUpInfoRequest, delegate (IMessage response)
        {
            GetHangUpInfoResponse getHangUpInfoResponse = response as GetHangUpInfoResponse;
            if (getHangUpInfoResponse != null && getHangUpInfoResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.HangUpDataModule).UpdateHangUpInfo(getHangUpInfoResponse.HungUpInfoDto);
                RedPointController.Instance.ReCalc("Main.HangUp", true);
                Action<bool, GetHangUpInfoResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, getHangUpInfoResponse);
                return;
            }
            else
            {
                Action<bool, GetHangUpInfoResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, getHangUpInfoResponse);
                return;
            }
        }, false, false, string.Empty, true);
    }

    public static void DoGetHangUpRewardRequest(bool isAd, Action<bool, GetHangUpRewardResponse> callback)
    {
        GetHangUpRewardRequest getHangUpRewardRequest = new GetHangUpRewardRequest();
        getHangUpRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
        getHangUpRewardRequest.Advert = (isAd ? 1 : 0);
        GameApp.NetWork.Send(getHangUpRewardRequest, delegate (IMessage response)
        {
            GetHangUpRewardResponse getHangUpRewardResponse = response as GetHangUpRewardResponse;
            if (getHangUpRewardResponse != null)
            {
                GameApp.Data.GetDataModule(DataName.HangUpDataModule).UpdateHangUpInfo(getHangUpRewardResponse.HungUpInfoDto);
            }
            if (getHangUpRewardResponse != null && getHangUpRewardResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.AdDataModule).UpdateAdData(getHangUpRewardResponse.CommonData.AdData);
                RedPointController.Instance.ReCalc("Main.HangUp", true);
                Action<bool, GetHangUpRewardResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, getHangUpRewardResponse);
                return;
            }
            else
            {
                Action<bool, GetHangUpRewardResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, getHangUpRewardResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoChapterBattlePassScoreRequest(int stage, long rowId, Action<bool, ChapterBattlePassScoreResponse> callback)
    {
        ChapterBattlePassScoreRequest chapterBattlePassScoreRequest = new ChapterBattlePassScoreRequest();
        chapterBattlePassScoreRequest.CommonParams = NetworkUtils.GetCommonParams();
        chapterBattlePassScoreRequest.Day = stage;
        chapterBattlePassScoreRequest.RowId = rowId;
        GameApp.NetWork.Send(chapterBattlePassScoreRequest, delegate (IMessage response)
        {
            ChapterBattlePassScoreResponse chapterBattlePassScoreResponse = response as ChapterBattlePassScoreResponse;
            if (chapterBattlePassScoreResponse != null && chapterBattlePassScoreResponse.Code == 0)
            {
                Singleton<EventRecordController>.Instance.EventGroupEnd();
                GameApp.Data.GetDataModule(DataName.ChapterBattlePassDataModule).UpdateScore(chapterBattlePassScoreResponse.Score, chapterBattlePassScoreResponse.RowId);
                Action<bool, ChapterBattlePassScoreResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, chapterBattlePassScoreResponse);
                return;
            }
            else
            {
                Action<bool, ChapterBattlePassScoreResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, chapterBattlePassScoreResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoGetChapterBattlePassRewardRequest(List<int> rewardIdList, Action<bool, GetChapterBattlePassRewardResponse> callback)
    {
        GetChapterBattlePassRewardRequest getChapterBattlePassRewardRequest = new GetChapterBattlePassRewardRequest();
        getChapterBattlePassRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
        for (int i = 0; i < rewardIdList.Count; i++)
        {
            getChapterBattlePassRewardRequest.RewardIdList.Add(rewardIdList[i]);
        }
        GameApp.NetWork.Send(getChapterBattlePassRewardRequest, delegate (IMessage response)
        {
            GetChapterBattlePassRewardResponse getChapterBattlePassRewardResponse = response as GetChapterBattlePassRewardResponse;
            if (getChapterBattlePassRewardResponse != null && getChapterBattlePassRewardResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.ChapterBattlePassDataModule).UpdateData(getChapterBattlePassRewardResponse.ChapterBattlePassDto);
                Action<bool, GetChapterBattlePassRewardResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, getChapterBattlePassRewardResponse);
                return;
            }
            else
            {
                Action<bool, GetChapterBattlePassRewardResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, getChapterBattlePassRewardResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoChapterBattlePassOpenBoxRequest(Action<bool, ChapterBattlePassOpenBoxResponse> callback)
    {
        ChapterBattlePassOpenBoxRequest chapterBattlePassOpenBoxRequest = new ChapterBattlePassOpenBoxRequest();
        chapterBattlePassOpenBoxRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(chapterBattlePassOpenBoxRequest, delegate (IMessage response)
        {
            ChapterBattlePassOpenBoxResponse chapterBattlePassOpenBoxResponse = response as ChapterBattlePassOpenBoxResponse;
            if (chapterBattlePassOpenBoxResponse != null && chapterBattlePassOpenBoxResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.ChapterBattlePassDataModule).UpdateFinalRewardCount(chapterBattlePassOpenBoxResponse.FinalRewardCount);
                Action<bool, ChapterBattlePassOpenBoxResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, chapterBattlePassOpenBoxResponse);
                return;
            }
            else
            {
                Action<bool, ChapterBattlePassOpenBoxResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, chapterBattlePassOpenBoxResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoChapterWheelScoreRequest(int stage, long rowId, Action<bool, ChapterWheelScoreResponse> callback)
    {
        ChapterWheelScoreRequest chapterWheelScoreRequest = new ChapterWheelScoreRequest();
        chapterWheelScoreRequest.CommonParams = NetworkUtils.GetCommonParams();
        chapterWheelScoreRequest.Day = stage;
        chapterWheelScoreRequest.RowId = rowId;
        GameApp.NetWork.Send(chapterWheelScoreRequest, delegate (IMessage response)
        {
            ChapterWheelScoreResponse chapterWheelScoreResponse = response as ChapterWheelScoreResponse;
            if (chapterWheelScoreResponse != null && chapterWheelScoreResponse.Code == 0)
            {
                ChapterActivityWheelDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterActivityWheelDataModule);
                int num = ((dataModule.WheelInfo != null) ? dataModule.WheelInfo.Score : 0);
                dataModule.UpdateScore(chapterWheelScoreResponse.RowId, chapterWheelScoreResponse.Score);
                Action<bool, ChapterWheelScoreResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, chapterWheelScoreResponse);
                }
                int num2 = ((dataModule.WheelInfo != null) ? dataModule.WheelInfo.Score : 0);
                int num3 = num2 - num;
                if (num3 > 0)
                {
                    string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(2, "activity_wheel_title");
                    GameApp.SDK.Analyze.Track_ActivityPoint(infoByID, num3, num2);
                    return;
                }
            }
            else
            {
                Action<bool, ChapterWheelScoreResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, chapterWheelScoreResponse);
            }
        }, true, false, string.Empty, true);
    }

    public static void DoChapterWheelSpineRequest(int rate, Action<bool, ChapterWheelSpineResponse> callback)
    {
        ChapterWheelSpineRequest chapterWheelSpineRequest = new ChapterWheelSpineRequest();
        chapterWheelSpineRequest.CommonParams = NetworkUtils.GetCommonParams();
        chapterWheelSpineRequest.Rate = rate;
        GameApp.NetWork.Send(chapterWheelSpineRequest, delegate (IMessage response)
        {
            ChapterWheelSpineResponse chapterWheelSpineResponse = response as ChapterWheelSpineResponse;
            if (chapterWheelSpineResponse != null && chapterWheelSpineResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.ChapterActivityWheelDataModule).UpdateSpinInfo(chapterWheelSpineResponse);
                Action<bool, ChapterWheelSpineResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, chapterWheelSpineResponse);
                }
                if (chapterWheelSpineResponse.CommonData != null)
                {
                    GameApp.SDK.Analyze.Track_Turntable(rate, chapterWheelSpineResponse.Score, chapterWheelSpineResponse.PlayTimes, chapterWheelSpineResponse.CommonData.Reward);
                    return;
                }
            }
            else
            {
                Action<bool, ChapterWheelSpineResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, chapterWheelSpineResponse);
            }
        }, true, false, string.Empty, true);
    }

    public static void DoChapterWheelInfoRequest(Action<bool, ChapterWheelInfoResponse> callback)
    {
        ChapterWheelInfoRequest chapterWheelInfoRequest = new ChapterWheelInfoRequest();
        chapterWheelInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(chapterWheelInfoRequest, delegate (IMessage response)
        {
            ChapterWheelInfoResponse chapterWheelInfoResponse = response as ChapterWheelInfoResponse;
            if (chapterWheelInfoResponse != null && chapterWheelInfoResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.ChapterActivityWheelDataModule).UpdateInfo(chapterWheelInfoResponse.Info);
                Action<bool, ChapterWheelInfoResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, chapterWheelInfoResponse);
                return;
            }
            else
            {
                Action<bool, ChapterWheelInfoResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, chapterWheelInfoResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }
}
