using System;
using System.Collections.Generic;

namespace HotFix.Client
{
	public class TaskManager
	{
		public void OnInit()
		{
		}

		public void OnUpdate(float deltaTime)
		{
			if (!this.m_isPlaying)
			{
				return;
			}
			this.m_currentTask = this.GetCurrentTask();
			if (this.m_currentTask == null)
			{
				return;
			}
			this.m_currentTask.Awake();
			this.m_currentTask.Start();
			if (this.m_currentTask.OnUpdate(deltaTime) == TaskStatus.Success)
			{
				this.m_currentTask.End();
				if (this.m_list.Contains(this.m_currentTask))
				{
					this.m_list.Remove(this.m_currentTask);
				}
			}
		}

		public void OnDeInit()
		{
			this.m_list.Clear();
		}

		private BaseTask GetCurrentTask()
		{
			if (this.m_list.Count <= 0)
			{
				return null;
			}
			return this.m_list[0];
		}

		public void AddTask(BaseTask task)
		{
			this.m_list.Add(task);
		}

		public void AddDelayCall(float delayTime, Action callback)
		{
			this.AddTask(new TaskDelay
			{
				m_delayTime = delayTime
			});
			this.AddTask(new TaskEvent
			{
				m_onEvent = delegate
				{
					Action callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2();
				}
			});
		}

		public void OnPlay()
		{
			this.m_isPlaying = true;
		}

		public void OnClear()
		{
			this.m_list.Clear();
		}

		public void OnStop()
		{
			this.m_isPlaying = false;
		}

		private List<BaseTask> m_list = new List<BaseTask>();

		private BaseTask m_currentTask;

		private bool m_isPlaying;
	}
}
