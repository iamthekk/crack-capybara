using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ActivityTurntable_TurntableQuestModelImpl : BaseLocalModelImpl<ActivityTurntable_TurntableQuest, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ActivityTurntable_TurntableQuest();
		}

		protected override int GetBeanKey(ActivityTurntable_TurntableQuest bean)
		{
			return bean.ID;
		}
	}
}
