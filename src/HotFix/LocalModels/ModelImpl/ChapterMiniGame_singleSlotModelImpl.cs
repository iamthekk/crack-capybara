using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChapterMiniGame_singleSlotModelImpl : BaseLocalModelImpl<ChapterMiniGame_singleSlot, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChapterMiniGame_singleSlot();
		}

		protected override int GetBeanKey(ChapterMiniGame_singleSlot bean)
		{
			return bean.id;
		}
	}
}
