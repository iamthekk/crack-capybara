using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class TowerChallenge_TowerModelImpl : BaseLocalModelImpl<TowerChallenge_Tower, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new TowerChallenge_Tower();
		}

		protected override int GetBeanKey(TowerChallenge_Tower bean)
		{
			return bean.id;
		}
	}
}
