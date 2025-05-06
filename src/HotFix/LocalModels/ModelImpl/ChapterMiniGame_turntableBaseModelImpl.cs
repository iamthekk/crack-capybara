using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChapterMiniGame_turntableBaseModelImpl : BaseLocalModelImpl<ChapterMiniGame_turntableBase, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChapterMiniGame_turntableBase();
		}

		protected override int GetBeanKey(ChapterMiniGame_turntableBase bean)
		{
			return bean.id;
		}
	}
}
