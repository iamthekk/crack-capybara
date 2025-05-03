using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Map_mapModelImpl : BaseLocalModelImpl<Map_map, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Map_map();
		}

		protected override int GetBeanKey(Map_map bean)
		{
			return bean.id;
		}
	}
}
