using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class IAP_MonthCardModelImpl : BaseLocalModelImpl<IAP_MonthCard, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new IAP_MonthCard();
		}

		protected override int GetBeanKey(IAP_MonthCard bean)
		{
			return bean.id;
		}
	}
}
