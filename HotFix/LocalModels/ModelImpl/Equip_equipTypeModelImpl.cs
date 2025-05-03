using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Equip_equipTypeModelImpl : BaseLocalModelImpl<Equip_equipType, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Equip_equipType();
		}

		protected override int GetBeanKey(Equip_equipType bean)
		{
			return bean.id;
		}
	}
}
