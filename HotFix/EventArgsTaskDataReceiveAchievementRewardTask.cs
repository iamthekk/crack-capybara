using System;
using Framework.EventSystem;
using Proto.Task;

namespace HotFix
{
	public class EventArgsTaskDataReceiveAchievementRewardTask : BaseEventArgs
	{
		public void SetData(int id, TaskRewardAchieveResponse response)
		{
			this.m_id = id;
			this.m_response = response;
		}

		public override void Clear()
		{
		}

		public TaskRewardAchieveResponse m_response;

		public int m_id;
	}
}
