using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChainPacks_ChainActvModelImpl : BaseLocalModelImpl<ChainPacks_ChainActv, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChainPacks_ChainActv();
		}

		protected override int GetBeanKey(ChainPacks_ChainActv bean)
		{
			return bean.id;
		}
	}
}
