using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChapterActivity_ModelModelImpl : BaseLocalModelImpl<ChapterActivity_Model, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChapterActivity_Model();
		}

		protected override int GetBeanKey(ChapterActivity_Model bean)
		{
			return bean.id;
		}
	}
}
