using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChapterMiniGame_slotBaseModelImpl : BaseLocalModelImpl<ChapterMiniGame_slotBase, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChapterMiniGame_slotBase();
		}

		protected override int GetBeanKey(ChapterMiniGame_slotBase bean)
		{
			return bean.id;
		}
	}
}
