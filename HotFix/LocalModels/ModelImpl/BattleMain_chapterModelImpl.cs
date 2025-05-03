using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class BattleMain_chapterModelImpl : BaseLocalModelImpl<BattleMain_chapter, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new BattleMain_chapter();
		}

		protected override int GetBeanKey(BattleMain_chapter bean)
		{
			return bean.ID;
		}
	}
}
