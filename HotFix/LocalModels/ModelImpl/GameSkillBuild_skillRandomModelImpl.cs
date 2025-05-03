using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GameSkillBuild_skillRandomModelImpl : BaseLocalModelImpl<GameSkillBuild_skillRandom, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GameSkillBuild_skillRandom();
		}

		protected override int GetBeanKey(GameSkillBuild_skillRandom bean)
		{
			return bean.id;
		}
	}
}
