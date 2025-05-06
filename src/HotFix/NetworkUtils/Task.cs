
public class Task
{
    public static void DoTaskGetInfoRequest(Action<bool, TaskGetInfoResponse> callback)
    {
        TaskGetInfoRequest taskGetInfoRequest = new TaskGetInfoRequest();
        taskGetInfoRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(taskGetInfoRequest, delegate (IMessage response)
        {
            TaskGetInfoResponse taskGetInfoResponse = response as TaskGetInfoResponse;
            if (taskGetInfoResponse != null && taskGetInfoResponse.Code == 0)
            {
                Action<bool, TaskGetInfoResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, taskGetInfoResponse);
                }
                RedPointController.Instance.ReCalc("Main.Mission", true);
                return;
            }
            Action<bool, TaskGetInfoResponse> callback3 = callback;
            if (callback3 != null)
            {
                callback3(false, taskGetInfoResponse);
            }
            HLog.LogError("发送任务-获取数据 失败:", (taskGetInfoResponse != null) ? taskGetInfoResponse.Code.ToString() : null);
        }, false, false, string.Empty, true);
    }

    public static void DoTaskRewardDailyRequest(int id, Action<bool, TaskRewardDailyResponse> callback = null)
    {
        TaskRewardDailyRequest taskRewardDailyRequest = new TaskRewardDailyRequest();
        taskRewardDailyRequest.CommonParams = NetworkUtils.GetCommonParams();
        taskRewardDailyRequest.Id = (uint)id;
        GameApp.NetWork.Send(taskRewardDailyRequest, delegate (IMessage response)
        {
            TaskRewardDailyResponse taskRewardDailyResponse = response as TaskRewardDailyResponse;
            if (taskRewardDailyResponse != null && taskRewardDailyResponse.Code == 0)
            {
                TaskDataModule dataModule = GameApp.Data.GetDataModule(DataName.TaskDataModule);
                uint activeDaily = taskRewardDailyResponse.ActiveDaily;
                int dailyActive = dataModule.DailyActive;
                EventArgsTaskDataReceiveRewardDailyTask instance = Singleton<EventArgsTaskDataReceiveRewardDailyTask>.Instance;
                instance.SetData(dataModule.DailyActive, dataModule.WeeklyActive, taskRewardDailyResponse);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_TaskDataModule_ReceiveRewardDailyTask, instance);
                Action<bool, TaskRewardDailyResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, taskRewardDailyResponse);
                }
                RedPointController.Instance.ReCalc("Main.Mission", true);
                GameApp.SDK.Analyze.Track_AtiveTask_CollectPoint();
                return;
            }
            Action<bool, TaskRewardDailyResponse> callback3 = callback;
            if (callback3 != null)
            {
                callback3(false, taskRewardDailyResponse);
            }
            HLog.LogError("发送任务-每日领取奖励 失败:", (taskRewardDailyResponse != null) ? taskRewardDailyResponse.Code.ToString() : null);
        }, true, false, string.Empty, true);
    }

    public static void DoTaskRewardAchieveRequest(int id, Action<bool, TaskRewardAchieveResponse> callback)
    {
        TaskRewardAchieveRequest taskRewardAchieveRequest = new TaskRewardAchieveRequest();
        taskRewardAchieveRequest.CommonParams = NetworkUtils.GetCommonParams();
        taskRewardAchieveRequest.Id = (uint)id;
        GameApp.NetWork.Send(taskRewardAchieveRequest, delegate (IMessage response)
        {
            TaskRewardAchieveResponse taskRewardAchieveResponse = response as TaskRewardAchieveResponse;
            if (taskRewardAchieveResponse != null && taskRewardAchieveResponse.Code == 0)
            {
                EventArgsTaskDataReceiveAchievementRewardTask instance = Singleton<EventArgsTaskDataReceiveAchievementRewardTask>.Instance;
                instance.SetData(id, taskRewardAchieveResponse);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_TaskDataModule_ReceiveAchievementRewardTask, instance);
                Action<bool, TaskRewardAchieveResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, taskRewardAchieveResponse);
                }
                RedPointController.Instance.ReCalc("Main.Mission", true);
                return;
            }
            Action<bool, TaskRewardAchieveResponse> callback3 = callback;
            if (callback3 != null)
            {
                callback3(false, taskRewardAchieveResponse);
            }
            HLog.LogError("发送成就-领取奖励 失败:", (taskRewardAchieveResponse != null) ? taskRewardAchieveResponse.Code.ToString() : null);
        }, true, false, string.Empty, true);
    }

    public static void DoTaskActiveRewardRequest(int id, int type, Action<bool, TaskActiveRewardResponse> callback)
    {
        TaskActiveRewardRequest taskActiveRewardRequest = new TaskActiveRewardRequest();
        taskActiveRewardRequest.CommonParams = NetworkUtils.GetCommonParams();
        taskActiveRewardRequest.Id = (uint)id;
        taskActiveRewardRequest.Type = (uint)type;
        GameApp.NetWork.Send(taskActiveRewardRequest, delegate (IMessage response)
        {
            TaskActiveRewardResponse taskActiveRewardResponse = response as TaskActiveRewardResponse;
            if (taskActiveRewardResponse != null && taskActiveRewardResponse.Code == 0)
            {
                Action<bool, TaskActiveRewardResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, taskActiveRewardResponse);
                }
                RedPointController.Instance.ReCalc("Main.Mission", true);
                return;
            }
            Action<bool, TaskActiveRewardResponse> callback3 = callback;
            if (callback3 != null)
            {
                callback3(false, taskActiveRewardResponse);
            }
            HLog.LogError("发送任务-领取活跃度奖励 失败:", (taskActiveRewardResponse != null) ? taskActiveRewardResponse.Code.ToString() : null);
        }, true, false, string.Empty, true);
    }

    public static void DoTaskActiveRewardAllRequest(int type, Action<bool, TaskActiveRewardAllResponse> callback)
    {
        TaskDataModule dataModule = GameApp.Data.GetDataModule(DataName.TaskDataModule);
        new List<int>();
        if (type != 1)
        {
            if (type != 2)
            {
                throw new ArgumentOutOfRangeException();
            }
            dataModule.GetCanWeeklyActiveReceiveIDs();
        }
        else
        {
            dataModule.GetCanDailyActiveReceiveIDs();
        }
        TaskActiveRewardAllRequest taskActiveRewardAllRequest = new TaskActiveRewardAllRequest();
        taskActiveRewardAllRequest.CommonParams = NetworkUtils.GetCommonParams();
        taskActiveRewardAllRequest.Type = (uint)type;
        GameApp.NetWork.Send(taskActiveRewardAllRequest, delegate (IMessage response)
        {
            TaskActiveRewardAllResponse taskActiveRewardAllResponse = response as TaskActiveRewardAllResponse;
            if (taskActiveRewardAllResponse != null && taskActiveRewardAllResponse.Code == 0)
            {
                EventArgsTaskDataReceiveActiveRewardAllTask instance = Singleton<EventArgsTaskDataReceiveActiveRewardAllTask>.Instance;
                instance.SetData(taskActiveRewardAllResponse);
                GameApp.Event.DispatchNow(null, LocalMessageName.CC_TaskDataModule_ReceiveActiveRewardAllTask, instance);
                Action<bool, TaskActiveRewardAllResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, taskActiveRewardAllResponse);
                }
                RedPointController.Instance.ReCalc("Main.Mission", true);
                GameApp.SDK.Analyze.Track_AtiveTask_CollectItem(taskActiveRewardAllResponse.CommonData.Reward);
                return;
            }
            Action<bool, TaskActiveRewardAllResponse> callback3 = callback;
            if (callback3 != null)
            {
                callback3(false, taskActiveRewardAllResponse);
            }
            HLog.LogError("发送任务-领取全活跃度奖励 失败:", (taskActiveRewardAllResponse != null) ? taskActiveRewardAllResponse.Code.ToString() : null);
        }, true, false, string.Empty, true);
    }
}
