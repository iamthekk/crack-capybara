using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Chapter_stageUpgradeModelImpl : BaseLocalModelImpl<Chapter_stageUpgrade, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Chapter_stageUpgrade();
		}

		protected override int GetBeanKey(Chapter_stageUpgrade bean)
		{
			return bean.id;
		}
	}
}
