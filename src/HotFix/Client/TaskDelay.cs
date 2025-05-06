using System;

namespace HotFix.Client
{
	public class TaskDelay : BaseTask
	{
		protected override void OnStart()
		{
			this.m_currentTime = 0f;
		}

		public override TaskStatus OnUpdate(float deltaTime)
		{
			this.m_currentTime += deltaTime;
			if (this.m_currentTime > this.m_delayTime)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Running;
		}

		public float m_delayTime;

		private float m_currentTime;
	}
}
