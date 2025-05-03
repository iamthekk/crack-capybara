using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GameSkill_hitEffectModelImpl : BaseLocalModelImpl<GameSkill_hitEffect, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GameSkill_hitEffect();
		}

		protected override int GetBeanKey(GameSkill_hitEffect bean)
		{
			return bean.id;
		}
	}
}
