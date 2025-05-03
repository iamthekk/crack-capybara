using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class MainLevelReward_AFKrewardModelImpl : BaseLocalModelImpl<MainLevelReward_AFKreward, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new MainLevelReward_AFKreward();
		}

		protected override int GetBeanKey(MainLevelReward_AFKreward bean)
		{
			return bean.ID;
		}
	}
}
