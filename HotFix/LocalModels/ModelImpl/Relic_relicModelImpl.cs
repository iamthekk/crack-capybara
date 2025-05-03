using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Relic_relicModelImpl : BaseLocalModelImpl<Relic_relic, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Relic_relic();
		}

		protected override int GetBeanKey(Relic_relic bean)
		{
			return bean.id;
		}
	}
}
