using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChapterActivity_ChapterActivityModelImpl : BaseLocalModelImpl<ChapterActivity_ChapterActivity, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChapterActivity_ChapterActivity();
		}

		protected override int GetBeanKey(ChapterActivity_ChapterActivity bean)
		{
			return bean.id;
		}
	}
}
