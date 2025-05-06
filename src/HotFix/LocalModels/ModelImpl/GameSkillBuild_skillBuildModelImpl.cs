using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GameSkillBuild_skillBuildModelImpl : BaseLocalModelImpl<GameSkillBuild_skillBuild, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GameSkillBuild_skillBuild();
		}

		protected override int GetBeanKey(GameSkillBuild_skillBuild bean)
		{
			return bean.id;
		}
	}
}
