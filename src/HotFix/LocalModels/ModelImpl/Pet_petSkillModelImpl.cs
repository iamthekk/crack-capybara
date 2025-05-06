using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Pet_petSkillModelImpl : BaseLocalModelImpl<Pet_petSkill, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Pet_petSkill();
		}

		protected override int GetBeanKey(Pet_petSkill bean)
		{
			return bean.id;
		}
	}
}
