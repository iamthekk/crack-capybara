using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class MainLevelReward_MainLevelChestModelImpl : BaseLocalModelImpl<MainLevelReward_MainLevelChest, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new MainLevelReward_MainLevelChest();
		}

		protected override int GetBeanKey(MainLevelReward_MainLevelChest bean)
		{
			return bean.id;
		}
	}
}
