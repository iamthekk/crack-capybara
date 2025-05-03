
public class Talent
{
    public static void DoAttributeUpgradeRequest(int step, string attributeKey, Action<bool, TalentsLvUpResponse> callback)
    {
        TalentsLvUpRequest talentsLvUpRequest = new TalentsLvUpRequest();
        talentsLvUpRequest.CommonParams = NetworkUtils.GetCommonParams();
        talentsLvUpRequest.AttributeType = attributeKey;
        talentsLvUpRequest.Step = (uint)step;
        GameApp.NetWork.Send(talentsLvUpRequest, delegate (IMessage response)
        {
            TalentsLvUpResponse talentsLvUpResponse = response as TalentsLvUpResponse;
            if (talentsLvUpResponse == null)
            {
                return;
            }
            if (talentsLvUpResponse.Code == 0)
            {
                if (talentsLvUpResponse.TalentsInfo != null)
                {
                    TalentDataModule dataModule = GameApp.Data.GetDataModule(DataName.TalentDataModule);
                    if ((ulong)talentsLvUpResponse.TalentsInfo.ExpProcess > (ulong)((long)dataModule.TalentExp))
                    {
                        dataModule.RefreshTalentData(talentsLvUpResponse.TalentsInfo);
                        RedPointController.Instance.ReCalc("Talent", true);
                        RedPointController.Instance.ReCalc("Main.NewWorld", true);
                        GameApp.Event.Dispatch(null, LocalMessageName.CC_UITalent_RefreshData, null);
                    }
                    Action<bool, TalentsLvUpResponse> callback2 = callback;
                    if (callback2 != null)
                    {
                        callback2(true, talentsLvUpResponse);
                    }
                    GameApp.SDK.Analyze.Track_TalentUp(attributeKey, talentsLvUpResponse.CommonData.CostDto);
                    return;
                }
                Action<bool, TalentsLvUpResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, talentsLvUpResponse);
                return;
            }
            else
            {
                Action<bool, TalentsLvUpResponse> callback4 = callback;
                if (callback4 == null)
                {
                    return;
                }
                callback4(false, talentsLvUpResponse);
                return;
            }
        }, false, false, string.Empty, true);
    }
}
