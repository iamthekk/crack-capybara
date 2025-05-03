using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class CrossArena_CrossArenaRobotModelImpl : BaseLocalModelImpl<CrossArena_CrossArenaRobot, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new CrossArena_CrossArenaRobot();
		}

		protected override int GetBeanKey(CrossArena_CrossArenaRobot bean)
		{
			return bean.ID;
		}
	}
}
