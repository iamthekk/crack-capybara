using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChapterActivity_ChapterObjModelImpl : BaseLocalModelImpl<ChapterActivity_ChapterObj, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChapterActivity_ChapterObj();
		}

		protected override int GetBeanKey(ChapterActivity_ChapterObj bean)
		{
			return bean.id;
		}
	}
}
