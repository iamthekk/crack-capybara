using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class CrossArena_CrossArenaRobotNameModelImpl : BaseLocalModelImpl<CrossArena_CrossArenaRobotName, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new CrossArena_CrossArenaRobotName();
		}

		protected override int GetBeanKey(CrossArena_CrossArenaRobotName bean)
		{
			return bean.Id;
		}
	}
}
