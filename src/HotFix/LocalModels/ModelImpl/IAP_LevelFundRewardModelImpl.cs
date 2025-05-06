using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class IAP_LevelFundRewardModelImpl : BaseLocalModelImpl<IAP_LevelFundReward, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new IAP_LevelFundReward();
		}

		protected override int GetBeanKey(IAP_LevelFundReward bean)
		{
			return bean.id;
		}
	}
}
