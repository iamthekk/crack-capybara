using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChapterMiniGame_turntableRewardModelImpl : BaseLocalModelImpl<ChapterMiniGame_turntableReward, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChapterMiniGame_turntableReward();
		}

		protected override int GetBeanKey(ChapterMiniGame_turntableReward bean)
		{
			return bean.id;
		}
	}
}
