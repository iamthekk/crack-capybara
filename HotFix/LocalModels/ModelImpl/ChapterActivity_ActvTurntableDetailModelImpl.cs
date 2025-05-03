using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChapterActivity_ActvTurntableDetailModelImpl : BaseLocalModelImpl<ChapterActivity_ActvTurntableDetail, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChapterActivity_ActvTurntableDetail();
		}

		protected override int GetBeanKey(ChapterActivity_ActvTurntableDetail bean)
		{
			return bean.id;
		}
	}
}
