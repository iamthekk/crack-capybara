using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Task_WeeklyActiveModelImpl : BaseLocalModelImpl<Task_WeeklyActive, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Task_WeeklyActive();
		}

		protected override int GetBeanKey(Task_WeeklyActive bean)
		{
			return bean.ID;
		}
	}
}
