using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class CrossArena_CrossArenaChallengeListRuleModelImpl : BaseLocalModelImpl<CrossArena_CrossArenaChallengeListRule, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new CrossArena_CrossArenaChallengeListRule();
		}

		protected override int GetBeanKey(CrossArena_CrossArenaChallengeListRule bean)
		{
			return bean.ID;
		}
	}
}
