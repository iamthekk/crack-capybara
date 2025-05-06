using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Mount_advanceMountModelImpl : BaseLocalModelImpl<Mount_advanceMount, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Mount_advanceMount();
		}

		protected override int GetBeanKey(Mount_advanceMount bean)
		{
			return bean.id;
		}
	}
}
