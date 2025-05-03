using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GameSkill_skillAnimationModelImpl : BaseLocalModelImpl<GameSkill_skillAnimation, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GameSkill_skillAnimation();
		}

		protected override int GetBeanKey(GameSkill_skillAnimation bean)
		{
			return bean.id;
		}
	}
}
