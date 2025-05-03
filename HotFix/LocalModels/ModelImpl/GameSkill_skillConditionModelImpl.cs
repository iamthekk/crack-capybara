using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GameSkill_skillConditionModelImpl : BaseLocalModelImpl<GameSkill_skillCondition, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GameSkill_skillCondition();
		}

		protected override int GetBeanKey(GameSkill_skillCondition bean)
		{
			return bean.id;
		}
	}
}
