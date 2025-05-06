using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class TGASource_IAPModelImpl : BaseLocalModelImpl<TGASource_IAP, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new TGASource_IAP();
		}

		protected override int GetBeanKey(TGASource_IAP bean)
		{
			return bean.id;
		}
	}
}
