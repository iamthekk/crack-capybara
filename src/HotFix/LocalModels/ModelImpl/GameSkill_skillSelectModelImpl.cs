using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GameSkill_skillSelectModelImpl : BaseLocalModelImpl<GameSkill_skillSelect, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GameSkill_skillSelect();
		}

		protected override int GetBeanKey(GameSkill_skillSelect bean)
		{
			return bean.id;
		}
	}
}
