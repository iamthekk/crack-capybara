using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Mining_showRateModelImpl : BaseLocalModelImpl<Mining_showRate, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Mining_showRate();
		}

		protected override int GetBeanKey(Mining_showRate bean)
		{
			return bean.id;
		}
	}
}
