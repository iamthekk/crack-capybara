using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GameSkill_skillModelImpl : BaseLocalModelImpl<GameSkill_skill, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GameSkill_skill();
		}

		protected override int GetBeanKey(GameSkill_skill bean)
		{
			return bean.id;
		}
	}
}
