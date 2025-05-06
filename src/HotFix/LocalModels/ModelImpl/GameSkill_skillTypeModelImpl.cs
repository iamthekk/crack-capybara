using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GameSkill_skillTypeModelImpl : BaseLocalModelImpl<GameSkill_skillType, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GameSkill_skillType();
		}

		protected override int GetBeanKey(GameSkill_skillType bean)
		{
			return bean.id;
		}
	}
}
