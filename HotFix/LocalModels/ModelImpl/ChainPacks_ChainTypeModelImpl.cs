using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ChainPacks_ChainTypeModelImpl : BaseLocalModelImpl<ChainPacks_ChainType, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ChainPacks_ChainType();
		}

		protected override int GetBeanKey(ChainPacks_ChainType bean)
		{
			return bean.id;
		}
	}
}
