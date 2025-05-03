using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Chapter_chapterModelImpl : BaseLocalModelImpl<Chapter_chapter, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Chapter_chapter();
		}

		protected override int GetBeanKey(Chapter_chapter bean)
		{
			return bean.id;
		}
	}
}
