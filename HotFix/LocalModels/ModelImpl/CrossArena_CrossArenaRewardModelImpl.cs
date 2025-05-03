using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class CrossArena_CrossArenaRewardModelImpl : BaseLocalModelImpl<CrossArena_CrossArenaReward, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new CrossArena_CrossArenaReward();
		}

		protected override int GetBeanKey(CrossArena_CrossArenaReward bean)
		{
			return bean.ID;
		}
	}
}
