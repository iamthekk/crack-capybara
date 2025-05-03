using System;

namespace HotFix.Client
{
	public class BaseTask
	{
		public void Awake()
		{
			if (this.isAwake)
			{
				return;
			}
			this.isAwake = true;
			this.OnAwake();
		}

		public void Start()
		{
			if (this.isStart)
			{
				return;
			}
			this.isStart = true;
			this.OnStart();
		}

		public void End()
		{
			this.isStart = false;
			this.OnEnd();
		}

		public void SetName(string name)
		{
			this.m_name = name;
		}

		public void SetSetParameters(string parameters)
		{
			this.OnSetParameters(parameters);
		}

		protected virtual void OnAwake()
		{
		}

		protected virtual void OnStart()
		{
		}

		protected virtual void OnEnd()
		{
		}

		protected virtual void OnSetParameters(string parameters)
		{
		}

		public virtual TaskStatus OnUpdate(float deltaTime)
		{
			return TaskStatus.Success;
		}

		protected string m_name = string.Empty;

		protected bool isAwake;

		protected bool isStart;
	}
}
