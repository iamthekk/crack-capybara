using System;
using System.Collections.Generic;

namespace HotFix.Client
{
	public class TaskParallel : TaskParent
	{
		protected override void OnAwake()
		{
			for (int i = 0; i < base.Count; i++)
			{
				this.children[i].Awake();
			}
		}

		protected override void OnStart()
		{
			this.m_results.Clear();
			for (int i = 0; i < base.Count; i++)
			{
				this.m_results.Add(false);
				this.children[i].Start();
			}
			this.m_successCount = 0;
		}

		public override TaskStatus OnUpdate(float deltaTime)
		{
			if (base.Count == 0)
			{
				return TaskStatus.Success;
			}
			for (int i = 0; i < base.Count; i++)
			{
				if (!this.m_results[i])
				{
					BaseTask baseTask = this.children[i];
					baseTask.Awake();
					baseTask.Start();
					if (baseTask.OnUpdate(deltaTime) == TaskStatus.Success)
					{
						baseTask.End();
						this.m_results[i] = true;
						this.m_successCount++;
					}
				}
			}
			if (this.m_successCount == base.Count)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Running;
		}

		protected override void OnEnd()
		{
		}

		private int m_successCount;

		private List<bool> m_results = new List<bool>();
	}
}
