using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChapterActivity_BattlepassQualityModelImpl : BaseLocalModelImpl<ChapterActivity_BattlepassQuality, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChapterActivity_BattlepassQuality();
		}

		protected override int GetBeanKey(ChapterActivity_BattlepassQuality bean)
		{
			return bean.id;
		}
	}
}
