using System;
using Framework.EventSystem;
using Proto.Task;

namespace HotFix
{
	public class EventArgsTaskDataReceiveActiveRewardAllTask : BaseEventArgs
	{
		public void SetData(TaskActiveRewardAllResponse response)
		{
			this.m_response = response;
		}

		public override void Clear()
		{
		}

		public TaskActiveRewardAllResponse m_response;
	}
}
