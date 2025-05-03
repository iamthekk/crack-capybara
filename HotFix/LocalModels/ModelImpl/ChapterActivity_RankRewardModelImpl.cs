using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChapterActivity_RankRewardModelImpl : BaseLocalModelImpl<ChapterActivity_RankReward, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChapterActivity_RankReward();
		}

		protected override int GetBeanKey(ChapterActivity_RankReward bean)
		{
			return bean.id;
		}
	}
}
