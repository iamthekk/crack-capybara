using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class SevenDay_SevenDayActiveRewardModelImpl : BaseLocalModelImpl<SevenDay_SevenDayActiveReward, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new SevenDay_SevenDayActiveReward();
		}

		protected override int GetBeanKey(SevenDay_SevenDayActiveReward bean)
		{
			return bean.ID;
		}
	}
}
