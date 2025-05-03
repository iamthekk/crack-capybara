using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class CrossArena_CrossArenaLevelModelImpl : BaseLocalModelImpl<CrossArena_CrossArenaLevel, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new CrossArena_CrossArenaLevel();
		}

		protected override int GetBeanKey(CrossArena_CrossArenaLevel bean)
		{
			return bean.Level;
		}
	}
}
