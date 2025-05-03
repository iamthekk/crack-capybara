using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Relic_updateLevelModelImpl : BaseLocalModelImpl<Relic_updateLevel, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Relic_updateLevel();
		}

		protected override int GetBeanKey(Relic_updateLevel bean)
		{
			return bean.id;
		}
	}
}
