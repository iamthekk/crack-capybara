using System;
using Framework.EventSystem;

namespace HotFix
{
	public interface IMemberState
	{
		int GetName();

		void OnEnter();

		void OnUpdate(float deltaTime, float unscaledDeltaTime);

		void OnExit();

		void RegisterEvents(EventSystemManager eventManager);

		void UnRegisterEvents(EventSystemManager eventManager);
	}
}
