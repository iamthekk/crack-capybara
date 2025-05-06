using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Equip_equipComposeModelImpl : BaseLocalModelImpl<Equip_equipCompose, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Equip_equipCompose();
		}

		protected override int GetBeanKey(Equip_equipCompose bean)
		{
			return bean.id;
		}
	}
}
