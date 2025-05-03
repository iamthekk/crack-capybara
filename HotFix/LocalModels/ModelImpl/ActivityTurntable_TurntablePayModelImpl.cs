using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ActivityTurntable_TurntablePayModelImpl : BaseLocalModelImpl<ActivityTurntable_TurntablePay, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ActivityTurntable_TurntablePay();
		}

		protected override int GetBeanKey(ActivityTurntable_TurntablePay bean)
		{
			return bean.id;
		}
	}
}
