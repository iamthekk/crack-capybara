using System;
using Framework.EventSystem;
using Proto.Task;

namespace HotFix
{
	public class EventArgsTaskDataReceiveRewardDailyTask : BaseEventArgs
	{
		public void SetData(int lastDailyActive, int lastWeeklyActive, TaskRewardDailyResponse response)
		{
			this.m_lastDailyActive = lastDailyActive;
			this.m_response = response;
		}

		public override void Clear()
		{
		}

		public int m_lastDailyActive;

		public int m_lastWeeklyActive;

		public TaskRewardDailyResponse m_response;
	}
}
