using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChapterMiniGame_paySlotRewardModelImpl : BaseLocalModelImpl<ChapterMiniGame_paySlotReward, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChapterMiniGame_paySlotReward();
		}

		protected override int GetBeanKey(ChapterMiniGame_paySlotReward bean)
		{
			return bean.id;
		}
	}
}
