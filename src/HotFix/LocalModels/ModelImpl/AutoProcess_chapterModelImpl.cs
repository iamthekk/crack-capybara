using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class AutoProcess_chapterModelImpl : BaseLocalModelImpl<AutoProcess_chapter, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new AutoProcess_chapter();
		}

		protected override int GetBeanKey(AutoProcess_chapter bean)
		{
			return bean.ID;
		}
	}
}
