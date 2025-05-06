using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class IAP_BattlePassModelImpl : BaseLocalModelImpl<IAP_BattlePass, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new IAP_BattlePass();
		}

		protected override int GetBeanKey(IAP_BattlePass bean)
		{
			return bean.id;
		}
	}
}
