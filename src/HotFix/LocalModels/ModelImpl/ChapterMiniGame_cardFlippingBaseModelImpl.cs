using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChapterMiniGame_cardFlippingBaseModelImpl : BaseLocalModelImpl<ChapterMiniGame_cardFlippingBase, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChapterMiniGame_cardFlippingBase();
		}

		protected override int GetBeanKey(ChapterMiniGame_cardFlippingBase bean)
		{
			return bean.id;
		}
	}
}
