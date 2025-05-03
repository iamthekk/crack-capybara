using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class NewWorld_newWorldTaskModelImpl : BaseLocalModelImpl<NewWorld_newWorldTask, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new NewWorld_newWorldTask();
		}

		protected override int GetBeanKey(NewWorld_newWorldTask bean)
		{
			return bean.id;
		}
	}
}
