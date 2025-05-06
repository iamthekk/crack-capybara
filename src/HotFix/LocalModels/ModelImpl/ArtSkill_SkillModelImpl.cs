using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ArtSkill_SkillModelImpl : BaseLocalModelImpl<ArtSkill_Skill, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ArtSkill_Skill();
		}

		protected override int GetBeanKey(ArtSkill_Skill bean)
		{
			return bean.id;
		}
	}
}
