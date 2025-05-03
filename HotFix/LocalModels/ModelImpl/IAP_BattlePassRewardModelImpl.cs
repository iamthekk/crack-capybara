using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class IAP_BattlePassRewardModelImpl : BaseLocalModelImpl<IAP_BattlePassReward, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new IAP_BattlePassReward();
		}

		protected override int GetBeanKey(IAP_BattlePassReward bean)
		{
			return bean.id;
		}
	}
}
