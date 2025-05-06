
public class Dungeon
{
    public static void DoStartDungeonRequest(int dungeonId, int levelId, bool isSweep, Action<bool, StartDungeonResponse> callback)
    {
        StartDungeonRequest startDungeonRequest = new StartDungeonRequest();
        startDungeonRequest.CommonParams = NetworkUtils.GetCommonParams();
        startDungeonRequest.DungeonId = dungeonId;
        startDungeonRequest.LevelId = levelId;
        startDungeonRequest.OptionType = (isSweep ? 1 : 0);
        startDungeonRequest.ClientVersion = Singleton<BattleServerVersionMgr>.Instance.GetVersion();
        GameApp.NetWork.Send(startDungeonRequest, delegate (IMessage response)
        {
            StartDungeonResponse startDungeonResponse = response as StartDungeonResponse;
            if (startDungeonResponse != null && startDungeonResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.DungeonDataModule).UpdateData(startDungeonResponse);
                Action<bool, StartDungeonResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, startDungeonResponse);
                }
                GameApp.SDK.Analyze.Track_Raid(dungeonId, levelId, isSweep, (int)startDungeonResponse.Result, startDungeonResponse.CommonData.CostDto, startDungeonResponse.CommonData.Reward);
                return;
            }
            Action<bool, StartDungeonResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, startDungeonResponse);
        }, true, false, string.Empty, true);
    }

    public static void DoDungeonAdGetItemRequest(int dungeonId, Action<bool, DungeonAdGetItemResponse> callback)
    {
        DungeonAdGetItemRequest dungeonAdGetItemRequest = new DungeonAdGetItemRequest();
        dungeonAdGetItemRequest.CommonParams = NetworkUtils.GetCommonParams();
        dungeonAdGetItemRequest.DungeonId = (uint)dungeonId;
        GameApp.NetWork.Send(dungeonAdGetItemRequest, delegate (IMessage response)
        {
            DungeonAdGetItemResponse dungeonAdGetItemResponse = response as DungeonAdGetItemResponse;
            if (dungeonAdGetItemResponse != null && dungeonAdGetItemResponse.Code == 0)
            {
                GameApp.Data.GetDataModule(DataName.AdDataModule).UpdateAdData(dungeonAdGetItemResponse.AdData);
                Action<bool, DungeonAdGetItemResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, dungeonAdGetItemResponse);
                return;
            }
            else
            {
                Action<bool, DungeonAdGetItemResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, dungeonAdGetItemResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }
}
