using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Equip_skillModelImpl : BaseLocalModelImpl<Equip_skill, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Equip_skill();
		}

		protected override int GetBeanKey(Equip_skill bean)
		{
			return bean.id;
		}
	}
}
