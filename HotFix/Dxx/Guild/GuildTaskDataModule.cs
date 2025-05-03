using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	[GuildInternalModule(15)]
	public class GuildTaskDataModule : IGuildModule
	{
		public int ModuleName
		{
			get
			{
				return 15;
			}
		}

		public List<GuildTaskData> TaskList
		{
			get
			{
				return this.mTaskList;
			}
		}

		public GuildTaskRefreshData RefreshData { get; private set; }

		public bool Init(GuildInitConfig config)
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
			@event.RegisterEvent(10, new GuildHandlerEvent(this.OnLoginSuccess));
			@event.RegisterEvent(16, new GuildHandlerEvent(this.OnSetMyGuildFeaturesInfo));
			@event.RegisterEvent(20, new GuildHandlerEvent(this.OnGuildTaskSetData));
			return true;
		}

		public void UnInit()
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
			@event.UnRegisterEvent(10, new GuildHandlerEvent(this.OnLoginSuccess));
			@event.UnRegisterEvent(16, new GuildHandlerEvent(this.OnSetMyGuildFeaturesInfo));
			@event.UnRegisterEvent(20, new GuildHandlerEvent(this.OnGuildTaskSetData));
		}

		private void OnLoginSuccess(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_LoginSuccess guildEvent_LoginSuccess = eventArgs as GuildEvent_LoginSuccess;
			if (guildEvent_LoginSuccess != null)
			{
				if (guildEvent_LoginSuccess.IsJoin)
				{
					this.mTaskList.Clear();
					if (guildEvent_LoginSuccess.TaskList != null)
					{
						this.mTaskList.AddRange(guildEvent_LoginSuccess.TaskList);
					}
					this.RefreshData = guildEvent_LoginSuccess.RefreshData;
					return;
				}
				this.mTaskList.Clear();
			}
		}

		private void OnSetMyGuildFeaturesInfo(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_SetMyGuildFeaturesInfo guildEvent_SetMyGuildFeaturesInfo = eventArgs as GuildEvent_SetMyGuildFeaturesInfo;
			if (guildEvent_SetMyGuildFeaturesInfo != null)
			{
				this.mTaskList.Clear();
				if (guildEvent_SetMyGuildFeaturesInfo.TaskList != null)
				{
					this.mTaskList.AddRange(guildEvent_SetMyGuildFeaturesInfo.TaskList);
				}
				if (guildEvent_SetMyGuildFeaturesInfo.RefreshData != null)
				{
					this.RefreshData = guildEvent_SetMyGuildFeaturesInfo.RefreshData;
				}
			}
		}

		private void OnGuildTaskSetData(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_GuildTaskSetData guildEvent_GuildTaskSetData = eventArgs as GuildEvent_GuildTaskSetData;
			if (guildEvent_GuildTaskSetData != null)
			{
				if (guildEvent_GuildTaskSetData.TaskList != null)
				{
					List<GuildTaskData> taskList = this.TaskList;
					List<GuildTaskData> taskList2 = guildEvent_GuildTaskSetData.TaskList;
					for (int i = 0; i < taskList2.Count; i++)
					{
						for (int j = 0; j < taskList.Count; j++)
						{
							if (taskList2[i].TaskId == taskList[j].TaskId)
							{
								taskList[j] = taskList2[i];
							}
						}
					}
				}
				if (guildEvent_GuildTaskSetData.DeleteTaskID >= 0)
				{
					List<GuildTaskData> taskList3 = this.TaskList;
					for (int k = 0; k < taskList3.Count; k++)
					{
						int taskId = taskList3[k].TaskId;
						int deleteTaskID = guildEvent_GuildTaskSetData.DeleteTaskID;
					}
				}
				if (guildEvent_GuildTaskSetData.RefreshData != null)
				{
					if (guildEvent_GuildTaskSetData.RefreshData.RefreshCost != null && this.RefreshData.RefreshCost != null)
					{
						if (guildEvent_GuildTaskSetData.RefreshData.RefreshCost.id > 0)
						{
							this.RefreshData.RefreshCost.id = guildEvent_GuildTaskSetData.RefreshData.RefreshCost.id;
						}
						this.RefreshData.RefreshCost.count = guildEvent_GuildTaskSetData.RefreshData.RefreshCost.count;
					}
					if (guildEvent_GuildTaskSetData.RefreshData.TaskRefreshCount >= 0)
					{
						this.RefreshData.TaskRefreshCount = guildEvent_GuildTaskSetData.RefreshData.TaskRefreshCount;
					}
				}
			}
		}

		private List<GuildTaskData> mTaskList = new List<GuildTaskData>();
	}
}
