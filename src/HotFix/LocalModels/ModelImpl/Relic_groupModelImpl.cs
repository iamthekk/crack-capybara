using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Relic_groupModelImpl : BaseLocalModelImpl<Relic_group, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Relic_group();
		}

		protected override int GetBeanKey(Relic_group bean)
		{
			return bean.id;
		}
	}
}
