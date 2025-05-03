using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class TGASource_ADModelImpl : BaseLocalModelImpl<TGASource_AD, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new TGASource_AD();
		}

		protected override int GetBeanKey(TGASource_AD bean)
		{
			return bean.id;
		}
	}
}
