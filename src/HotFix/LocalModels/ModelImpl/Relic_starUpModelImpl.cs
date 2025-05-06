using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Relic_starUpModelImpl : BaseLocalModelImpl<Relic_starUp, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Relic_starUp();
		}

		protected override int GetBeanKey(Relic_starUp bean)
		{
			return bean.id;
		}
	}
}
