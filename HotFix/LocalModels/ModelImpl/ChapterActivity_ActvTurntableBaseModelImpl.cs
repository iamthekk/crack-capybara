using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChapterActivity_ActvTurntableBaseModelImpl : BaseLocalModelImpl<ChapterActivity_ActvTurntableBase, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChapterActivity_ActvTurntableBase();
		}

		protected override int GetBeanKey(ChapterActivity_ActvTurntableBase bean)
		{
			return bean.id;
		}
	}
}
