using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChapterMiniGame_slotTrainBuildModelImpl : BaseLocalModelImpl<ChapterMiniGame_slotTrainBuild, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChapterMiniGame_slotTrainBuild();
		}

		protected override int GetBeanKey(ChapterMiniGame_slotTrainBuild bean)
		{
			return bean.id;
		}
	}
}
