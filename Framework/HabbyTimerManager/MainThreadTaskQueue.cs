using System;
using System.Collections.Generic;

namespace Framework.HabbyTimerManager
{
	public class MainThreadTaskQueue
	{
		public static void EnqueueTask(Action action)
		{
			object obj = MainThreadTaskQueue.queueLock;
			lock (obj)
			{
				MainThreadTaskQueue.tasks.Enqueue(action);
			}
		}

		public static void ExecuteTasks()
		{
			if (MainThreadTaskQueue.tasks.Count > 0)
			{
				object obj = MainThreadTaskQueue.queueLock;
				Queue<Action> queue;
				lock (obj)
				{
					queue = new Queue<Action>(MainThreadTaskQueue.tasks);
					MainThreadTaskQueue.tasks.Clear();
					goto IL_0057;
				}
				IL_003E:
				Action action = queue.Dequeue();
				try
				{
					if (action != null)
					{
						action();
					}
				}
				catch (Exception ex)
				{
					HLog.LogException(ex);
				}
				IL_0057:
				if (queue.Count > 0)
				{
					goto IL_003E;
				}
			}
		}

		private static readonly Queue<Action> tasks = new Queue<Action>();

		private static readonly object queueLock = new object();
	}
}
