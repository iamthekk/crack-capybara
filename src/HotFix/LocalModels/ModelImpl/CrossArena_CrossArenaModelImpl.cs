using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class CrossArena_CrossArenaModelImpl : BaseLocalModelImpl<CrossArena_CrossArena, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new CrossArena_CrossArena();
		}

		protected override int GetBeanKey(CrossArena_CrossArena bean)
		{
			return bean.id;
		}
	}
}
