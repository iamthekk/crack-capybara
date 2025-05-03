using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class CrossArena_CrossArenaTimeModelImpl : BaseLocalModelImpl<CrossArena_CrossArenaTime, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new CrossArena_CrossArenaTime();
		}

		protected override int GetBeanKey(CrossArena_CrossArenaTime bean)
		{
			return bean.ID;
		}
	}
}
