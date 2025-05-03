using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChapterActivity_ActvTurntableRewardModelImpl : BaseLocalModelImpl<ChapterActivity_ActvTurntableReward, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChapterActivity_ActvTurntableReward();
		}

		protected override int GetBeanKey(ChapterActivity_ActvTurntableReward bean)
		{
			return bean.id;
		}
	}
}
