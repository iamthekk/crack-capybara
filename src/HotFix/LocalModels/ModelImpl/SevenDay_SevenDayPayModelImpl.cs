using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class SevenDay_SevenDayPayModelImpl : BaseLocalModelImpl<SevenDay_SevenDayPay, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new SevenDay_SevenDayPay();
		}

		protected override int GetBeanKey(SevenDay_SevenDayPay bean)
		{
			return bean.id;
		}
	}
}
