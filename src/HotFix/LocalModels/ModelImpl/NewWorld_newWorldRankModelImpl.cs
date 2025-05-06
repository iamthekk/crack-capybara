using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class NewWorld_newWorldRankModelImpl : BaseLocalModelImpl<NewWorld_newWorldRank, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new NewWorld_newWorldRank();
		}

		protected override int GetBeanKey(NewWorld_newWorldRank bean)
		{
			return bean.id;
		}
	}
}
