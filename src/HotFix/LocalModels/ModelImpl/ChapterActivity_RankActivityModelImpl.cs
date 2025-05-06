using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChapterActivity_RankActivityModelImpl : BaseLocalModelImpl<ChapterActivity_RankActivity, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChapterActivity_RankActivity();
		}

		protected override int GetBeanKey(ChapterActivity_RankActivity bean)
		{
			return bean.id;
		}
	}
}
