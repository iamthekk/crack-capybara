using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChapterMiniGame_singleSlotRewardModelImpl : BaseLocalModelImpl<ChapterMiniGame_singleSlotReward, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChapterMiniGame_singleSlotReward();
		}

		protected override int GetBeanKey(ChapterMiniGame_singleSlotReward bean)
		{
			return bean.id;
		}
	}
}
