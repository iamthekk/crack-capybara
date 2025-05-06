using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class BattleMain_skillModelImpl : BaseLocalModelImpl<BattleMain_skill, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new BattleMain_skill();
		}

		protected override int GetBeanKey(BattleMain_skill bean)
		{
			return bean.ID;
		}
	}
}
