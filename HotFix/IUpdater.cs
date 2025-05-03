using System;

namespace HotFix
{
	public interface IUpdater
	{
		void OnInit();

		void OnUpdate(float deltaTime, float unscaledDeltaTime);

		void OnDeInit();

		void OnDispose();
	}
}
