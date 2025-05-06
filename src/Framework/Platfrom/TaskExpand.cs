using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Platfrom
{
	public class TaskExpand
	{
		public static async Task Delay(int millionSecond)
		{
			await Task.Delay(millionSecond);
		}

		public static async Task Yield()
		{
			await Task.Yield();
		}

		public static async Task WhenAll(IEnumerable<Task> tasks)
		{
			await Task.WhenAll(tasks);
		}

		public static async Task Run(Action action)
		{
			await Task.Run(action);
		}
	}
}
