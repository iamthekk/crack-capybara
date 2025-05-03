using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Mining_qualityModelModelImpl : BaseLocalModelImpl<Mining_qualityModel, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Mining_qualityModel();
		}

		protected override int GetBeanKey(Mining_qualityModel bean)
		{
			return bean.id;
		}
	}
}
