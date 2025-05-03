using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChestList_ChestRewardModelImpl : BaseLocalModelImpl<ChestList_ChestReward, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChestList_ChestReward();
		}

		protected override int GetBeanKey(ChestList_ChestReward bean)
		{
			return bean.id;
		}
	}
}
