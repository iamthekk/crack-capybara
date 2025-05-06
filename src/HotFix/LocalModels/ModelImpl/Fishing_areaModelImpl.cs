using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Fishing_areaModelImpl : BaseLocalModelImpl<Fishing_area, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Fishing_area();
		}

		protected override int GetBeanKey(Fishing_area bean)
		{
			return bean.id;
		}
	}
}
