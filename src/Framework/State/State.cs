using System;
using Framework.EventSystem;

namespace Framework.State
{
	public abstract class State
	{
		public abstract int GetName();

		public abstract void OnEnter();

		public abstract void OnExit();

		public abstract void OnUpdate(float deltaTime, float unscaledDeltaTime);

		public virtual void OnLateUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public virtual void RegisterEvents(EventSystemManager manager)
		{
		}

		public virtual void UnRegisterEvents(EventSystemManager manager)
		{
		}
	}
}
