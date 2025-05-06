using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class TGASource_GetModelImpl : BaseLocalModelImpl<TGASource_Get, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new TGASource_Get();
		}

		protected override int GetBeanKey(TGASource_Get bean)
		{
			return bean.id;
		}
	}
}
