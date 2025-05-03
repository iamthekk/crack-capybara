using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChapterMiniGame_slotRewardModelImpl : BaseLocalModelImpl<ChapterMiniGame_slotReward, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChapterMiniGame_slotReward();
		}

		protected override int GetBeanKey(ChapterMiniGame_slotReward bean)
		{
			return bean.id;
		}
	}
}
