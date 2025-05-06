using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Fishing_fishModelImpl : BaseLocalModelImpl<Fishing_fish, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Fishing_fish();
		}

		protected override int GetBeanKey(Fishing_fish bean)
		{
			return bean.id;
		}
	}
}
