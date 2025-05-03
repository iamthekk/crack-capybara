using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Equip_equipSkillModelImpl : BaseLocalModelImpl<Equip_equipSkill, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Equip_equipSkill();
		}

		protected override int GetBeanKey(Equip_equipSkill bean)
		{
			return bean.id;
		}
	}
}
