using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class TGASource_CostModelImpl : BaseLocalModelImpl<TGASource_Cost, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new TGASource_Cost();
		}

		protected override int GetBeanKey(TGASource_Cost bean)
		{
			return bean.id;
		}
	}
}
