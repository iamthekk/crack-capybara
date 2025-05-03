using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Fishing_fishingModelImpl : BaseLocalModelImpl<Fishing_fishing, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Fishing_fishing();
		}

		protected override int GetBeanKey(Fishing_fishing bean)
		{
			return bean.id;
		}
	}
}
