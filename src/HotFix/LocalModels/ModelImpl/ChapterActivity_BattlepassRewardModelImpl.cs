using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChapterActivity_BattlepassRewardModelImpl : BaseLocalModelImpl<ChapterActivity_BattlepassReward, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChapterActivity_BattlepassReward();
		}

		protected override int GetBeanKey(ChapterActivity_BattlepassReward bean)
		{
			return bean.id;
		}
	}
}
