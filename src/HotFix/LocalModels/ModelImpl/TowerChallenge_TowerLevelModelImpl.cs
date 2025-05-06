using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class TowerChallenge_TowerLevelModelImpl : BaseLocalModelImpl<TowerChallenge_TowerLevel, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new TowerChallenge_TowerLevel();
		}

		protected override int GetBeanKey(TowerChallenge_TowerLevel bean)
		{
			return bean.id;
		}
	}
}
