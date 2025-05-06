using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class IAP_LevelFundModelImpl : BaseLocalModelImpl<IAP_LevelFund, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new IAP_LevelFund();
		}

		protected override int GetBeanKey(IAP_LevelFund bean)
		{
			return bean.id;
		}
	}
}
