using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Achievements_AchievementsModelImpl : BaseLocalModelImpl<Achievements_Achievements, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Achievements_Achievements();
		}

		protected override int GetBeanKey(Achievements_Achievements bean)
		{
			return bean.ID;
		}
	}
}
