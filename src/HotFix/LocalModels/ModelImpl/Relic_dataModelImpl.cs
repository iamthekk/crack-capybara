using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Relic_dataModelImpl : BaseLocalModelImpl<Relic_data, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Relic_data();
		}

		protected override int GetBeanKey(Relic_data bean)
		{
			return bean.id;
		}
	}
}
