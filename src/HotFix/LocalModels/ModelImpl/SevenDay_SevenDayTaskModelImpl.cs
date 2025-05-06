using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class SevenDay_SevenDayTaskModelImpl : BaseLocalModelImpl<SevenDay_SevenDayTask, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new SevenDay_SevenDayTask();
		}

		protected override int GetBeanKey(SevenDay_SevenDayTask bean)
		{
			return bean.ID;
		}
	}
}
