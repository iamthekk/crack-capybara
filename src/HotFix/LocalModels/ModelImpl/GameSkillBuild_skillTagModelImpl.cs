using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GameSkillBuild_skillTagModelImpl : BaseLocalModelImpl<GameSkillBuild_skillTag, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GameSkillBuild_skillTag();
		}

		protected override int GetBeanKey(GameSkillBuild_skillTag bean)
		{
			return bean.id;
		}
	}
}
