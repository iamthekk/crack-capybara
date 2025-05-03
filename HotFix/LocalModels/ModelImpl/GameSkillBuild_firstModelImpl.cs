using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GameSkillBuild_firstModelImpl : BaseLocalModelImpl<GameSkillBuild_first, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GameSkillBuild_first();
		}

		protected override int GetBeanKey(GameSkillBuild_first bean)
		{
			return bean.id;
		}
	}
}
