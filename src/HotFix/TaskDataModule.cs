using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Common;
using Proto.Task;
using UnityEngine;

namespace HotFix
{
	public class TaskDataModule : IDataModule
	{
		public List<TaskDataModule.TaskDailyData> DailyDatas
		{
			get
			{
				return this.m_dailyDatas;
			}
		}

		public List<TaskDataModule.TaskActive> DailyTaskActiveDatas
		{
			get
			{
				return this.m_dailyTaskActiveDatas;
			}
		}

		public List<TaskDataModule.TaskActive> WeeklyTaskActiveDatas
		{
			get
			{
				return this.m_weeklyTaskActiveDatas;
			}
		}

		public long DailyTaskResetTime
		{
			get
			{
				return this.m_dailyTaskResetTime;
			}
		}

		public int DailyActive
		{
			get
			{
				return this.m_dailyActive;
			}
		}

		public int DailyMaxActive
		{
			get
			{
				return this.m_dailyMaxActive;
			}
		}

		public ulong DailyTaskRewardLog
		{
			get
			{
				return this.m_dailyTaskRewardLog;
			}
		}

		public bool IsHaveDailyActiveReceive
		{
			get
			{
				return this.m_isHaveDailyActiveReceive;
			}
		}

		public bool IsDailyActiveFinished
		{
			get
			{
				return this.m_isDailyActiveFinished;
			}
		}

		public int WeeklyActive
		{
			get
			{
				return this.m_weeklyActive;
			}
		}

		public int WeeklyMaxActive
		{
			get
			{
				return this.m_weeklyMaxActive;
			}
		}

		public ulong WeeklyTaskRewardLog
		{
			get
			{
				return this.m_weeklyTaskRewardLog;
			}
		}

		public bool IsHaveWeeklyActiveReceive
		{
			get
			{
				return this.m_isHaveWeeklyActiveReceive;
			}
		}

		public bool IsWeeklyActiveFinished
		{
			get
			{
				return this.m_isWeeklyActiveFinished;
			}
		}

		public List<TaskDataModule.AchievementData> AchievementDatas
		{
			get
			{
				return this.m_achievementDatas;
			}
		}

		public int GetName()
		{
			return 140;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_DayChange_Task_DataPull, new HandlerEvent(this.OnEventDayChangeTaskDataPull));
			manager.RegisterEvent(LocalMessageName.CC_TaskDataModule_LoadTaskData, new HandlerEvent(this.OnEventRefreshTaskData));
			manager.RegisterEvent(LocalMessageName.CC_TaskDataModule_ReceiveRewardDailyTask, new HandlerEvent(this.OnEventReceiveRewardDailyTask));
			manager.RegisterEvent(LocalMessageName.CC_TaskDataModule_ReceiveActiveRewardAllTask, new HandlerEvent(this.OnEventReceiveActiveRewardAllTask));
			manager.RegisterEvent(LocalMessageName.CC_TaskDataModule_ReceiveAchievementRewardTask, new HandlerEvent(this.OnEventReceiveAchievementRewardTask));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_DayChange_Task_DataPull, new HandlerEvent(this.OnEventDayChangeTaskDataPull));
			manager.UnRegisterEvent(LocalMessageName.CC_TaskDataModule_LoadTaskData, new HandlerEvent(this.OnEventRefreshTaskData));
			manager.UnRegisterEvent(LocalMessageName.CC_TaskDataModule_ReceiveRewardDailyTask, new HandlerEvent(this.OnEventReceiveRewardDailyTask));
			manager.UnRegisterEvent(LocalMessageName.CC_TaskDataModule_ReceiveActiveRewardAllTask, new HandlerEvent(this.OnEventReceiveActiveRewardAllTask));
			manager.UnRegisterEvent(LocalMessageName.CC_TaskDataModule_ReceiveAchievementRewardTask, new HandlerEvent(this.OnEventReceiveAchievementRewardTask));
		}

		public void Reset()
		{
		}

		private void OnRefreshTaskData(TaskGetInfoResponse response)
		{
			if (response == null)
			{
				return;
			}
			this.m_dailyTaskResetTime = (long)response.DailyTaskResetTime;
			this.m_dailyActive = (int)response.DailyTaskActive;
			this.m_weeklyActive = (int)response.WeeklyTaskActive;
			this.m_dailyMaxActive = 1;
			this.m_weeklyMaxActive = 1;
			this.m_dailyTaskRewardLog = response.DailyTaskRewardLog;
			this.m_weeklyTaskRewardLog = response.WeeklyTaskRewardLog;
			this.OnInitDailyDatas();
			if (response.Tasks != null)
			{
				this.OnRefreshDailyDatas(response.Tasks.DailyTask.ToList<TaskDto>());
			}
			this.OnInitDailyActiveDatas();
			this.OnRefreshDailyActiveDatas();
			this.OnInitWeeklyActiveDatas();
			this.OnRefreshWeeklyActiveDatas();
			this.InitAchievements();
			if (response.Tasks != null)
			{
				this.OnRefreshAchievementDatas(response.Tasks.Achievements.ToList<TaskDto>(), null);
			}
		}

		private void OnRefreshTaskData(TaskRewardDailyResponse response)
		{
			if (response == null)
			{
				return;
			}
			this.m_dailyActive = (int)response.ActiveDaily;
			this.m_weeklyActive = (int)response.ActiveWeekly;
			if (response.UpdateTaskDto != null && response.UpdateTaskDto.Id != 0U)
			{
				this.OnRefreshDailyDatas(new List<TaskDto> { response.UpdateTaskDto });
			}
			if (response.Tasks != null)
			{
				if (response.Tasks.DailyTask != null)
				{
					this.OnRefreshDailyDatas(response.Tasks.DailyTask.ToList<TaskDto>());
				}
				if (response.Tasks.Achievements != null)
				{
					this.OnRefreshAchievementDatas(response.Tasks.Achievements.ToList<TaskDto>(), null);
				}
			}
			this.OnRefreshDailyActiveDatas();
			this.OnRefreshWeeklyActiveDatas();
		}

		private void OnRefreshTaskData(TaskActiveRewardAllResponse response)
		{
			if (response == null)
			{
				return;
			}
			if (response.Tasks != null)
			{
				if (response.Tasks.DailyTask != null)
				{
					this.OnRefreshDailyDatas(response.Tasks.DailyTask.ToList<TaskDto>());
				}
				if (response.Tasks.Achievements != null)
				{
					this.OnRefreshAchievementDatas(response.Tasks.Achievements.ToList<TaskDto>(), null);
				}
			}
			switch (response.Type)
			{
			case 0U:
			case 3U:
				break;
			case 1U:
				this.m_dailyTaskRewardLog = response.RewardLog;
				this.OnRefreshDailyActiveDatas();
				return;
			case 2U:
				this.m_weeklyTaskRewardLog = response.RewardLog;
				this.OnRefreshWeeklyActiveDatas();
				break;
			default:
				return;
			}
		}

		private void OnRefreshTaskData(TaskRewardAchieveResponse response)
		{
			if (response == null)
			{
				return;
			}
			List<TaskDto> list = new List<TaskDto>();
			List<uint> list2 = new List<uint>();
			if (response.UpdateTaskDto != null && response.UpdateTaskDto.Id != 0U)
			{
				list.Add(response.UpdateTaskDto);
			}
			if (response.DeleteTaskDtoId != 0U)
			{
				list2.Add(response.DeleteTaskDtoId);
			}
			if (list.Count != 0 || list2.Count != 0)
			{
				this.OnRefreshAchievementDatas(list, list2);
			}
			if (response.Tasks != null)
			{
				if (response.Tasks.DailyTask != null)
				{
					this.OnRefreshDailyDatas(response.Tasks.DailyTask.ToList<TaskDto>());
				}
				if (response.Tasks.Achievements != null)
				{
					this.OnRefreshAchievementDatas(response.Tasks.Achievements.ToList<TaskDto>(), null);
				}
			}
		}

		private void OnInitDailyDatas()
		{
			this.m_dailyDatasDic.Clear();
			this.DailyDatas.Clear();
			IList<Task_DailyTask> allElements = GameApp.Table.GetManager().GetTask_DailyTaskModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				Task_DailyTask task_DailyTask = allElements[i];
				if (task_DailyTask != null && task_DailyTask.ID != 0 && task_DailyTask.UnlockNeed != -1)
				{
					TaskDataModule.TaskDailyData taskDailyData = new TaskDataModule.TaskDailyData();
					taskDailyData.ID = task_DailyTask.ID;
					taskDailyData.taskType = TaskDataModule.TaskType.DailyTask;
					taskDailyData.dailyTaskType = (TaskDataModule.DailyTaskType)task_DailyTask.DailyType;
					taskDailyData.DailyNeedCount = task_DailyTask.DailyNeed;
					taskDailyData.DailyNeedParam = task_DailyTask.DailyNeedParam;
					taskDailyData.DailyDescribe = task_DailyTask.DailyDescribe.ToString();
					taskDailyData.DailyActiveReward = task_DailyTask.DailyActiveReward;
					taskDailyData.JumpViewID = task_DailyTask.Jump;
					taskDailyData.UnlockNeed = task_DailyTask.UnlockNeed;
					this.m_dailyDatasDic[task_DailyTask.ID] = taskDailyData;
					this.DailyDatas.Add(taskDailyData);
				}
			}
		}

		private void OnRefreshDailyDatas(List<TaskDto> datas)
		{
			for (int i = 0; i < datas.Count; i++)
			{
				TaskDto taskDto = datas[i];
				TaskDataModule.TaskDailyData taskDailyData;
				if (taskDto != null && this.m_dailyDatasDic.TryGetValue((int)taskDto.Id, out taskDailyData))
				{
					taskDailyData.IsComplete = taskDto.IsFinish;
					taskDailyData.IsAward = taskDto.IsReceive;
					taskDailyData.DailyCompleteCount = (int)taskDto.Process;
				}
			}
			IOrderedEnumerable<TaskDataModule.TaskDailyData> orderedEnumerable = from taskData in this.DailyDatas
				orderby taskData.IsCompleteAndNoAward() descending, taskData.IsComplete, taskData.ID
				select taskData;
			this.m_dailyDatas = orderedEnumerable.ToList<TaskDataModule.TaskDailyData>();
		}

		public void RefreshDataFromServer(RepeatedField<TaskDto> updateTasks)
		{
			List<TaskDto> list = new List<TaskDto>();
			List<TaskDto> list2 = new List<TaskDto>();
			for (int i = 0; i < updateTasks.Count; i++)
			{
				TaskDataModule.TaskType taskType = (TaskDataModule.TaskType)updateTasks[i].TaskType;
				if (taskType != TaskDataModule.TaskType.DailyTask)
				{
					if (taskType == TaskDataModule.TaskType.Achievement)
					{
						list2.Add(updateTasks[i]);
					}
				}
				else
				{
					list.Add(updateTasks[i]);
				}
			}
			if (list.Count > 0)
			{
				this.OnRefreshDailyDatas(list);
			}
			if (list2.Count > 0)
			{
				this.OnRefreshAchievementDatas(list2, null);
			}
		}

		private void OnInitDailyActiveDatas()
		{
			this.m_dailyTaskActiveDatas.Clear();
			this.m_dailyMaxActive = 1;
			IList<Task_DailyActive> allElements = GameApp.Table.GetManager().GetTask_DailyActiveModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				Task_DailyActive task_DailyActive = allElements[i];
				TaskDataModule.TaskActive taskActive = new TaskDataModule.TaskActive();
				taskActive.ID = task_DailyActive.ID;
				taskActive.NeedActive = task_DailyActive.Requirements;
				taskActive.Reward = task_DailyActive.Reward;
				this.m_dailyTaskActiveDatas.Add(taskActive);
				if (this.DailyMaxActive < task_DailyActive.Requirements)
				{
					this.m_dailyMaxActive = task_DailyActive.Requirements;
				}
			}
		}

		private void OnRefreshDailyActiveDatas()
		{
			this.m_isDailyActiveFinished = true;
			this.m_isHaveDailyActiveReceive = false;
			for (int i = 0; i < this.m_dailyTaskActiveDatas.Count; i++)
			{
				TaskDataModule.TaskActive taskActive = this.m_dailyTaskActiveDatas[i];
				if (taskActive != null)
				{
					taskActive.IsReward = this.isTrue((long)this.DailyTaskRewardLog, taskActive.ID);
					if (this.DailyActive >= taskActive.NeedActive && !taskActive.IsReward && !this.IsHaveDailyActiveReceive)
					{
						this.m_isHaveDailyActiveReceive = true;
					}
					if (!taskActive.IsReward)
					{
						this.m_isDailyActiveFinished = false;
					}
				}
			}
		}

		public List<int> GetCanDailyActiveReceiveIDs()
		{
			List<int> list = new List<int>();
			if (this.m_isDailyActiveFinished)
			{
				return list;
			}
			for (int i = 0; i < this.m_dailyTaskActiveDatas.Count; i++)
			{
				TaskDataModule.TaskActive taskActive = this.m_dailyTaskActiveDatas[i];
				if (taskActive != null && this.DailyActive >= taskActive.NeedActive && !taskActive.IsReward)
				{
					list.Add(taskActive.ID);
				}
			}
			return list;
		}

		private void OnInitWeeklyActiveDatas()
		{
			this.WeeklyTaskActiveDatas.Clear();
			this.m_weeklyMaxActive = 1;
			IList<Task_WeeklyActive> allElements = GameApp.Table.GetManager().GetTask_WeeklyActiveModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				Task_WeeklyActive task_WeeklyActive = allElements[i];
				TaskDataModule.TaskActive taskActive = new TaskDataModule.TaskActive();
				taskActive.ID = task_WeeklyActive.ID;
				taskActive.NeedActive = task_WeeklyActive.Requirements;
				taskActive.Reward = task_WeeklyActive.FixReward;
				this.WeeklyTaskActiveDatas.Add(taskActive);
				if (this.WeeklyMaxActive < task_WeeklyActive.Requirements)
				{
					this.m_weeklyMaxActive = task_WeeklyActive.Requirements;
				}
			}
		}

		private void OnRefreshWeeklyActiveDatas()
		{
			this.m_isWeeklyActiveFinished = true;
			this.m_isHaveWeeklyActiveReceive = false;
			for (int i = 0; i < this.WeeklyTaskActiveDatas.Count; i++)
			{
				TaskDataModule.TaskActive taskActive = this.WeeklyTaskActiveDatas[i];
				if (taskActive != null)
				{
					taskActive.IsReward = this.isTrue((long)this.WeeklyTaskRewardLog, taskActive.ID);
					if (this.WeeklyActive >= taskActive.NeedActive && !taskActive.IsReward && !this.IsHaveWeeklyActiveReceive)
					{
						this.m_isHaveWeeklyActiveReceive = true;
					}
					if (!taskActive.IsReward)
					{
						this.m_isWeeklyActiveFinished = false;
					}
				}
			}
		}

		public List<int> GetCanWeeklyActiveReceiveIDs()
		{
			List<int> list = new List<int>();
			if (this.m_isWeeklyActiveFinished)
			{
				return list;
			}
			for (int i = 0; i < this.WeeklyTaskActiveDatas.Count; i++)
			{
				TaskDataModule.TaskActive taskActive = this.WeeklyTaskActiveDatas[i];
				if (taskActive != null && this.WeeklyActive >= taskActive.NeedActive && !taskActive.IsReward)
				{
					list.Add(taskActive.ID);
				}
			}
			return list;
		}

		public void InitAchievements()
		{
			this.m_achievementDic.Clear();
			this.m_achievementDatas.Clear();
		}

		private TaskDataModule.AchievementData InitAchievementData(Achievements_Achievements achievements)
		{
			return new TaskDataModule.AchievementData
			{
				ID = achievements.ID,
				AchievementType = achievements.AchievementsType,
				ProgressType = achievements.ProgressType,
				accumulationType = (TaskDataModule.AccumulationType)achievements.AccumulationType,
				AchievementsLevel = achievements.AchievementsLevel,
				AchievementNeedCount = achievements.AchievementsNeed,
				AchievementDescribe = achievements.AchievementsDescribe.ToString(),
				Reward = achievements.Reward,
				UnlockNeed = achievements.UnlockNeed
			};
		}

		private void OnRefreshAchievementDatas(List<TaskDto> datas, List<uint> deleteIds)
		{
			if (datas != null)
			{
				for (int i = 0; i < datas.Count; i++)
				{
					TaskDto taskDto = datas[i];
					if (taskDto != null)
					{
						int id = (int)taskDto.Id;
						TaskDataModule.AchievementData achievementData;
						this.m_achievementDic.TryGetValue(id, out achievementData);
						if (achievementData == null)
						{
							Achievements_Achievements elementById = GameApp.Table.GetManager().GetAchievements_AchievementsModelInstance().GetElementById(id);
							if (elementById == null)
							{
								goto IL_009C;
							}
							achievementData = this.InitAchievementData(elementById);
							this.m_achievementDatas.Add(achievementData);
						}
						achievementData.IsReceive = taskDto.IsReceive;
						achievementData.AchievementCompleteCount = (int)taskDto.Process;
						achievementData.IsComplete = taskDto.IsFinish;
						this.m_achievementDic[id] = achievementData;
					}
					IL_009C:;
				}
			}
			if (deleteIds != null)
			{
				List<TaskDataModule.AchievementData> list = new List<TaskDataModule.AchievementData>();
				for (int j = 0; j < deleteIds.Count; j++)
				{
					uint num = deleteIds[j];
					if (num > 0U)
					{
						TaskDataModule.AchievementData achievementData2;
						this.m_achievementDic.TryGetValue((int)num, out achievementData2);
						if (achievementData2 != null)
						{
							list.Add(achievementData2);
						}
					}
				}
				for (int k = 0; k < list.Count; k++)
				{
					TaskDataModule.AchievementData achievementData3 = list[k];
					if (achievementData3 != null)
					{
						this.m_achievementDic.Remove(achievementData3.ID);
						this.m_achievementDatas.Remove(achievementData3);
					}
				}
			}
			IOrderedEnumerable<TaskDataModule.AchievementData> orderedEnumerable = from data in this.m_achievementDatas
				orderby data.IsCompleteAndNoAward() descending, data.IsComplete, data.ID
				select data;
			this.m_achievementDatas = orderedEnumerable.ToList<TaskDataModule.AchievementData>();
		}

		public bool GetIsCanReceiveForTaskTask()
		{
			for (int i = 0; i < this.m_dailyDatas.Count; i++)
			{
				TaskDataModule.TaskDailyData taskDailyData = this.m_dailyDatas[i];
				if (taskDailyData != null && taskDailyData.IsCompleteAndNoAward())
				{
					return true;
				}
			}
			return this.m_isHaveDailyActiveReceive || this.m_isHaveWeeklyActiveReceive;
		}

		public bool GetIsCanReceiveForAchievement()
		{
			for (int i = 0; i < this.m_achievementDatas.Count; i++)
			{
				TaskDataModule.AchievementData achievementData = this.m_achievementDatas[i];
				if (achievementData != null && achievementData.IsCompleteAndNoAward())
				{
					return true;
				}
			}
			return false;
		}

		private bool isTrue(long mask, int position)
		{
			if (mask == 0L)
			{
				return false;
			}
			long num = 1L << position;
			return (mask & num) == num;
		}

		private void OnEventDayChangeTaskDataPull(object sender, int type, BaseEventArgs eventargs)
		{
			NetworkUtils.Task.DoTaskGetInfoRequest(delegate(bool isOk, TaskGetInfoResponse rep)
			{
				if (isOk)
				{
					this.OnRefreshTaskData(rep);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_Task_Refresh, null);
				}
			});
		}

		private void OnEventRefreshTaskData(object sender, int type, BaseEventArgs eventargs)
		{
			NetworkUtils.Task.DoTaskGetInfoRequest(delegate(bool isOk, TaskGetInfoResponse rep)
			{
				if (!isOk)
				{
					return;
				}
				this.OnRefreshTaskData(rep);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_TaskDataModule_RefreshTaskData, null);
			});
		}

		private void OnEventReceiveRewardDailyTask(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsTaskDataReceiveRewardDailyTask eventArgsTaskDataReceiveRewardDailyTask = eventargs as EventArgsTaskDataReceiveRewardDailyTask;
			if (eventArgsTaskDataReceiveRewardDailyTask == null || eventArgsTaskDataReceiveRewardDailyTask.m_response == null)
			{
				return;
			}
			this.OnRefreshTaskData(eventArgsTaskDataReceiveRewardDailyTask.m_response);
		}

		private void OnEventReceiveActiveRewardAllTask(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsTaskDataReceiveActiveRewardAllTask eventArgsTaskDataReceiveActiveRewardAllTask = eventargs as EventArgsTaskDataReceiveActiveRewardAllTask;
			if (eventArgsTaskDataReceiveActiveRewardAllTask == null || eventArgsTaskDataReceiveActiveRewardAllTask.m_response == null)
			{
				return;
			}
			this.OnRefreshTaskData(eventArgsTaskDataReceiveActiveRewardAllTask.m_response);
		}

		private void OnEventReceiveAchievementRewardTask(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsTaskDataReceiveAchievementRewardTask eventArgsTaskDataReceiveAchievementRewardTask = eventargs as EventArgsTaskDataReceiveAchievementRewardTask;
			if (eventArgsTaskDataReceiveAchievementRewardTask == null || eventArgsTaskDataReceiveAchievementRewardTask.m_response == null)
			{
				return;
			}
			this.OnRefreshTaskData(eventArgsTaskDataReceiveAchievementRewardTask.m_response);
		}

		private Dictionary<int, TaskDataModule.TaskDailyData> m_dailyDatasDic = new Dictionary<int, TaskDataModule.TaskDailyData>();

		[SerializeField]
		private List<TaskDataModule.TaskDailyData> m_dailyDatas = new List<TaskDataModule.TaskDailyData>();

		[SerializeField]
		private List<TaskDataModule.TaskActive> m_dailyTaskActiveDatas = new List<TaskDataModule.TaskActive>();

		[SerializeField]
		private List<TaskDataModule.TaskActive> m_weeklyTaskActiveDatas = new List<TaskDataModule.TaskActive>();

		[SerializeField]
		private long m_dailyTaskResetTime;

		[SerializeField]
		private int m_dailyActive;

		[SerializeField]
		private int m_dailyMaxActive = 1;

		[SerializeField]
		private ulong m_dailyTaskRewardLog;

		[SerializeField]
		private bool m_isHaveDailyActiveReceive;

		[SerializeField]
		private bool m_isDailyActiveFinished;

		[SerializeField]
		private int m_weeklyActive;

		[SerializeField]
		private int m_weeklyMaxActive = 1;

		[SerializeField]
		private ulong m_weeklyTaskRewardLog;

		[SerializeField]
		private bool m_isHaveWeeklyActiveReceive;

		[SerializeField]
		private bool m_isWeeklyActiveFinished;

		private const long defaultLongValue = 1L;

		private Dictionary<int, TaskDataModule.AchievementData> m_achievementDic = new Dictionary<int, TaskDataModule.AchievementData>();

		[SerializeField]
		private List<TaskDataModule.AchievementData> m_achievementDatas = new List<TaskDataModule.AchievementData>();

		[RuntimeDefaultSerializedProperty]
		public class TaskDailyData
		{
			public bool IsUnlock()
			{
				return GameApp.Data.GetDataModule(DataName.MainDataModule).ChapterMaxProcess >= this.UnlockNeed;
			}

			public bool IsCompleteAndNoAward()
			{
				return this.IsComplete && !this.IsAward;
			}

			public int ID;

			public TaskDataModule.TaskType taskType;

			public TaskDataModule.DailyTaskType dailyTaskType;

			public int DailyNeedCount;

			public int DailyCompleteCount;

			public int DailyNeedParam;

			public string DailyDescribe;

			public int DailyActiveReward;

			public int JumpViewID;

			public int UnlockNeed;

			public bool IsAward;

			public bool IsComplete;

			public int SortValue;
		}

		public enum TaskType
		{
			Null,
			DailyTask,
			WeeklyTask,
			Achievement
		}

		[RuntimeDefaultSerializedProperty]
		public class TaskActive
		{
			public int ID;

			public int NeedActive;

			public string[] Reward;

			public bool IsReward;
		}

		public enum DailyTaskType
		{
			Null,
			ChallengeCheckpoint,
			CardLevelUp,
			EquipmentLevelUp,
			QuickIdle,
			QuickIdleAward,
			DrawCard,
			Tower,
			Maze,
			Dispatch,
			MiniGames
		}

		[RuntimeDefaultSerializedProperty]
		public class AchievementData
		{
			public bool IsUnlock()
			{
				return GameApp.Data.GetDataModule(DataName.MainDataModule).ChapterMaxProcess >= this.UnlockNeed;
			}

			public bool IsCompleteAndNoAward()
			{
				return this.IsComplete && !this.IsReceive;
			}

			public string GetAchievementDescribeParameter()
			{
				int achievementType = this.AchievementType;
				if (achievementType != 8)
				{
					if (achievementType != 9)
					{
						return this.AchievementNeedCount.ToString();
					}
					CrossArena_CrossArenaLevel elementById = GameApp.Table.GetManager().GetCrossArena_CrossArenaLevelModelInstance().GetElementById(this.AchievementNeedCount);
					if (elementById != null)
					{
						return Singleton<LanguageManager>.Instance.GetInfoByID(elementById.name);
					}
					return string.Empty;
				}
				else
				{
					TowerChallenge_Tower elementById2 = GameApp.Table.GetManager().GetTowerChallenge_TowerModelInstance().GetElementById(this.AchievementNeedCount);
					if (elementById2 != null)
					{
						return Singleton<LanguageManager>.Instance.GetInfoByID(elementById2.name);
					}
					return string.Empty;
				}
			}

			public string GetProgressInfo()
			{
				if (this.ProgressType == 0)
				{
					return string.Format("{0}/1", this.AchievementCompleteCount);
				}
				return string.Format("{0}/{1}", this.AchievementCompleteCount, this.AchievementNeedCount);
			}

			public int ID;

			public TaskDataModule.TaskType taskType = TaskDataModule.TaskType.Achievement;

			public int AchievementType;

			public int ProgressType;

			public TaskDataModule.AccumulationType accumulationType;

			public int AchievementsLevel;

			public int AchievementNeedCount;

			public int AchievementCompleteCount;

			public bool IsComplete;

			public string AchievementDescribe;

			public bool IsReceive;

			public string[] Reward;

			public int UnlockNeed;
		}

		public enum AccumulationType
		{
			No,
			Add
		}
	}
}
