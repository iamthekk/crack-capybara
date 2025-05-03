using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ActivityTurntable_ActivityTurntableModelImpl : BaseLocalModelImpl<ActivityTurntable_ActivityTurntable, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ActivityTurntable_ActivityTurntable();
		}

		protected override int GetBeanKey(ActivityTurntable_ActivityTurntable bean)
		{
			return bean.id;
		}
	}
}
