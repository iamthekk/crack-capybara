using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Vip_dataModelImpl : BaseLocalModelImpl<Vip_data, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Vip_data();
		}

		protected override int GetBeanKey(Vip_data bean)
		{
			return bean.id;
		}
	}
}
