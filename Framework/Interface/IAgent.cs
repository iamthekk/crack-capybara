using System;

namespace Framework.Interface
{
	public interface IAgent<T> where T : ITask
	{
		T CurrentTask();

		void AddTask(T task);

		void OnUpdate(float deltaTime, float unscaledDeltaTime);

		void RemoveTask();
	}
}
