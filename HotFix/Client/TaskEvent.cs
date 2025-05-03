using System;

namespace HotFix.Client
{
	public class TaskEvent : BaseTask
	{
		public override TaskStatus OnUpdate(float deltaTime)
		{
			Action onEvent = this.m_onEvent;
			if (onEvent != null)
			{
				onEvent();
			}
			return TaskStatus.Success;
		}

		public Action m_onEvent;
	}
}
