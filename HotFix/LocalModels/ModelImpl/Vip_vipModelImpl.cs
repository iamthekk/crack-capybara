using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Vip_vipModelImpl : BaseLocalModelImpl<Vip_vip, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Vip_vip();
		}

		protected override int GetBeanKey(Vip_vip bean)
		{
			return bean.id;
		}
	}
}
