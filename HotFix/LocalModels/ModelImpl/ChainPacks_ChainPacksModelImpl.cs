using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChainPacks_ChainPacksModelImpl : BaseLocalModelImpl<ChainPacks_ChainPacks, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChainPacks_ChainPacks();
		}

		protected override int GetBeanKey(ChainPacks_ChainPacks bean)
		{
			return bean.id;
		}
	}
}
