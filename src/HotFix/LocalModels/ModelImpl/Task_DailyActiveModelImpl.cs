using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Task_DailyActiveModelImpl : BaseLocalModelImpl<Task_DailyActive, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Task_DailyActive();
		}

		protected override int GetBeanKey(Task_DailyActive bean)
		{
			return bean.ID;
		}
	}
}
