using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Equip_equipModelImpl : BaseLocalModelImpl<Equip_equip, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Equip_equip();
		}

		protected override int GetBeanKey(Equip_equip bean)
		{
			return bean.id;
		}
	}
}
