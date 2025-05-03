using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChapterMiniGame_paySlotBaseModelImpl : BaseLocalModelImpl<ChapterMiniGame_paySlotBase, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChapterMiniGame_paySlotBase();
		}

		protected override int GetBeanKey(ChapterMiniGame_paySlotBase bean)
		{
			return bean.id;
		}
	}
}
