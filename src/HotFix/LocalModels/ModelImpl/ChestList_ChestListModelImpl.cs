using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChestList_ChestListModelImpl : BaseLocalModelImpl<ChestList_ChestList, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChestList_ChestList();
		}

		protected override int GetBeanKey(ChestList_ChestList bean)
		{
			return bean.id;
		}
	}
}
