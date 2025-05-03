using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Task_DailyTaskModelImpl : BaseLocalModelImpl<Task_DailyTask, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Task_DailyTask();
		}

		protected override int GetBeanKey(Task_DailyTask bean)
		{
			return bean.ID;
		}
	}
}
