using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChainPacks_PushChainActvModelImpl : BaseLocalModelImpl<ChainPacks_PushChainActv, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChainPacks_PushChainActv();
		}

		protected override int GetBeanKey(ChainPacks_PushChainActv bean)
		{
			return bean.id;
		}
	}
}
