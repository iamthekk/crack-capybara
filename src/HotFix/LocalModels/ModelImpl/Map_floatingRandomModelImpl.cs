using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Map_floatingRandomModelImpl : BaseLocalModelImpl<Map_floatingRandom, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Map_floatingRandom();
		}

		protected override int GetBeanKey(Map_floatingRandom bean)
		{
			return bean.id;
		}
	}
}
