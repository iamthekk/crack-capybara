using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ActivityTurntable_TurntableRewardModelImpl : BaseLocalModelImpl<ActivityTurntable_TurntableReward, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ActivityTurntable_TurntableReward();
		}

		protected override int GetBeanKey(ActivityTurntable_TurntableReward bean)
		{
			return bean.id;
		}
	}
}
